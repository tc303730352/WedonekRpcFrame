using System;
using System.Collections.Concurrent;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Modular.Config;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;

namespace WeDonekRpc.Modular.Domain
{
    internal class UserAccreditDomain
    {
        /// <summary>
        /// 有效时间
        /// </summary>
        private int _VaildTime = 0;
        /// <summary>
        /// 下次与服务端同步时间
        /// </summary>
        private int _CheckTime = 0;
        /// <summary>
        /// 状态版本号
        /// </summary>
        private int _StateVer = 0;
        /// <summary>
        /// 同步校验Key
        /// </summary>
        private string _CheckKey = null;
        /// <summary>
        /// 状态值
        /// </summary>
        private readonly ConcurrentDictionary<string, IAccreditState> _UserState = [];
        /// <summary>
        /// 状态JSON字符串
        /// </summary>
        private string _StateJson = null;
        /// <summary>
        /// 心跳时间
        /// </summary>
        private int _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;

        /// <summary>
        ///  创建者所属服务组别
        /// </summary>
        private readonly string _SysGroup;
        /// <summary>
        /// 创建者集群Id
        /// </summary>
        private readonly long _RpcMerId;
        private readonly int _Expire;

        public UserAccreditDomain (AccreditDatum datum) : this(datum.StateVer, datum.Accredit, datum.State)
        {
        }
        public UserAccreditDomain (int stateVer, AccreditRes res, string state)
        {
            this._Expire = res.Expire.HasValue ? HeartbeatTimeHelper.GetTime(res.Expire.Value) : int.MaxValue;
            this.ApplyId = res.ApplyId;
            this.AccreditId = res.AccreditId;
            this._SysGroup = res.SysGroup;
            this._RpcMerId = res.RpcMerId;
            this._StateJson = state;
            this._Init(stateVer, res);
        }
        public string ApplyId
        {
            get;
        }
        public string AccreditId
        {
            get;
            private set;
        }

        public bool IsError
        {
            get;
            private set;
        }
        public string Error
        {
            get;
            private set;
        }
        public string StateJson { get => this._StateJson; }

        public IUserState GetUserState (Type type)
        {
            if (this._UserState.TryGetValue(type.FullName, out IAccreditState state))
            {
                return state;
            }
            else if (this._StateJson == null)
            {
                return null;
            }
            state = (IAccreditState)this._StateJson.Json(type);
            state.BindAccreditId(this.AccreditId, this._SysGroup, this._RpcMerId);
            if (this._UserState.TryAdd(type.FullName, state))
            {
                return state;
            }
            return this.GetUserState(type);
        }

        private void _Init (int stateVer, AccreditRes res)
        {
            this._StateVer = stateVer;
            this._CheckKey = res.CheckKey;
            this._VaildTime = ModularConfig.Accredit.GetCacheVaildTime();
            this._CheckTime = ModularConfig.Accredit.GetNextCheckTime(HeartbeatTimeHelper.HeartbeatTime);
        }
        public void CancelAccredit ()
        {
            if (this.IsError)
            {
                return;
            }
            CancelAccredit obj = new CancelAccredit
            {
                AccreditId = this.AccreditId,
                CheckKey = this._CheckKey
            };
            obj.Send();
            this.Error = "accredit.overdue";
            this.IsError = true;
        }


        public bool CheckStatus (int time)
        {
            if (this._Expire <= time)
            {
                return false;
            }
            if (this._VaildTime <= time)
            {
                return false;
            }
            else if (this._CheckTime <= time && !this.IsError)
            {
                return this._CheckAccredit(time);
            }
            return true;
        }

        private bool _CheckAccredit (int time)
        {
            bool isRefresh = ( time - this._HeartbeatTime ) < ModularConfig.Accredit.RefreshTime;
            if (new CheckAccredit
            {
                AccreditId = this.AccreditId,
                CheckKey = this._CheckKey,
                IsRefresh = isRefresh
            }.Send(out int ver, out string error))
            {
                return this._StateVer == ver;
            }
            else if (!error.StartsWith("socket"))
            {
                this.Error = error;
                this.IsError = true;
                this._VaildTime = time + ModularConfig.Accredit.ErrorVaildTime;
            }
            this._CheckTime = ModularConfig.Accredit.GetNextCheckTime(time);
            return true;
        }
        private SetUserStateRes _SetUserState (IUserState state)
        {
            return new SetUserState
            {
                AccreditId = this.AccreditId,
                UserState = state.ToJson(),
                VerNum = Interlocked.CompareExchange(ref this._StateVer, 0, 0)
            }.Send();
        }
        public void ToUpdate (IUserState state, int? expire = null)
        {
            string json = state?.ToJson();
            ApplyAccreditRes res = new SetAccredit
            {
                AccreditId = this.AccreditId,
                State = json,
                Expire = expire
            }.Send();
            this._StateJson = json;
            this._Init(res.StateVer, res.Accredit);
        }
        public bool SetUserState (IUserState state)
        {
            SetUserStateRes result = this._SetUserState(state);
            this._Save(result);
            return result.IsSuccess;
        }
        private void _Clear ()
        {
            this._UserState.Clear();
        }
        public bool SetUserState (IUserState state, Func<IUserState, IUserState, IUserState> upFun)
        {
            return this._SetUserState(state, upFun, 0);
        }
        private void _Save (SetUserStateRes result)
        {
            _ = Interlocked.Exchange(ref this._StateVer, result.StateVer);
            this._StateJson = result.UserState;
            this._Clear();
        }
        private bool _SetUserState (IUserState state, Func<IUserState, IUserState, IUserState> upFun, int retryNum)
        {
            SetUserStateRes result = this._SetUserState(state);
            if (!result.IsSuccess)
            {
                if (upFun == null || retryNum == 2)
                {
                    this._Save(result);
                    return false;
                }
                IUserState newState = (IUserState)JsonTools.Json(result.UserState, state.GetType());
                IUserState t = upFun(newState, state);
                if (t == null || t.Equals(newState))
                {
                    this._Save(result);
                    return true;
                }
                else
                {
                    return this._SetUserState(t, upFun, retryNum + 1);
                }
            }
            this._Save(result);
            return true;
        }

        internal void Refresh ()
        {
            _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
        }
    }
}
