using System;
using System.Collections.Generic;
using System.Threading;

using RpcModel;

using RpcModular.Config;

using RpcModularModel.Accredit;
using RpcModularModel.Accredit.Model;

using RpcHelper;

namespace RpcModular.Domain
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
                private long _StateVer = 0;
                /// <summary>
                /// 同步校验Key
                /// </summary>
                private string _CheckKey = null;
                /// <summary>
                /// 状态值
                /// </summary>
                private readonly Dictionary<string, IAccreditState> _UserState = new Dictionary<string, IAccreditState>();
                /// <summary>
                /// 状态JSON字符串
                /// </summary>
                private string _StateJson = null;
                /// <summary>
                /// 心跳时间
                /// </summary>
                private readonly int _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;

                private readonly LockHelper _Lock = new LockHelper();
                /// <summary>
                ///  创建者所属服务组别
                /// </summary>
                private readonly long _SysGroupId;
                /// <summary>
                /// 创建者集群Id
                /// </summary>
                private readonly long _RpcMerId;

                public UserAccreditDomain(AccreditDatum datum) : this(datum.ApplyId, datum.StateVer, datum.Accredit, datum.State)
                {
                }
                public UserAccreditDomain(string applyId, long stateVer, AccreditRes res, string state)
                {
                        this.ApplyId = applyId;
                        this.AccreditId = res.AccreditId;
                        this._SysGroupId = res.SysGroupId;
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


                public IUserState GetUserState(Type type)
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
                        state.BindAccreditId(this.AccreditId, this._SysGroupId, this._RpcMerId);
                        if (this._Lock.GetLock())
                        {
                                this._UserState.TryAdd(type.FullName, state);
                                this._Lock.Exit();
                        }
                        return state;
                }

                private void _Init(long stateVer, AccreditRes res)
                {
                        this._StateVer = stateVer;
                        this._CheckKey = res.CheckKey;
                        this._VaildTime = ModularConfig.Accredit.GetCacheVaildTime();
                        this._CheckTime = ModularConfig.Accredit.GetNextCheckTime();
                }
                public void CancelAccredit()
                {
                        if (this.IsError)
                        {
                                return;
                        }
                        CancelAccredit obj = new CancelAccredit
                        {
                                AccreditId = AccreditId,
                                CheckKey = _CheckKey
                        };
                        obj.Send();
                        this.Error = "accredit.overdue";
                        this.IsError = true;
                }

                public bool CheckStatus()
                {
                        int time = HeartbeatTimeHelper.HeartbeatTime;
                        if (this._VaildTime <= time)
                        {
                                return false;
                        }
                        else if (this._CheckTime <= time && !this.IsError)
                        {
                                bool isRefresh = (HeartbeatTimeHelper.HeartbeatTime - this._HeartbeatTime) < ModularConfig.Accredit.HeartbeatTime;
                                if (!_CheckAccredit(this.AccreditId, this._CheckKey, isRefresh, out string error))
                                {
                                        if (!error.StartsWith("socket"))
                                        {
                                                this.Error = error;
                                                this.IsError = true;
                                                this._VaildTime = HeartbeatTimeHelper.HeartbeatTime + ModularConfig.Accredit.ErrorVaildTime;
                                        }
                                }
                                else
                                {
                                        this._CheckTime = ModularConfig.Accredit.GetNextCheckTime();
                                }
                        }
                        return true;
                }

                private static bool _CheckAccredit(string accreditId, string key, bool isRefresh, out string error)
                {
                        return new CheckAccredit
                        {
                                AccreditId = accreditId,
                                CheckKey = key,
                                IsRefresh = isRefresh
                        }.Send(out error);
                }
                private SetUserStateRes _SetUserState(IUserState state, bool IsCover)
                {
                        return new SetUserState
                        {
                                AccreditId = AccreditId,
                                UserState = state.ToJson(),
                                VerNum = Interlocked.Read(ref this._StateVer),
                                IsCover = IsCover
                        }.Send();
                }
                public void ToUpdate(IUserState state, string[] roleId)
                {
                        string json = state?.ToJson();
                        ApplyAccreditRes res = new SetAccredit
                        {
                                AccreditId = AccreditId,
                                AccreditRole = roleId ?? new string[0],
                                State = json
                        }.Send();
                        this._StateJson = json;
                        this._Init(res.StateVer, res.Accredit);
                }
                public void SetUserState(IUserState state)
                {
                        SetUserStateRes result = this._SetUserState(state, false);
                        Interlocked.Exchange(ref this._StateVer, result.StateVer);
                        this._StateJson = result.UserState;
                        this._Clear();
                }
                private void _Clear()
                {
                        if (this._Lock.GetLock())
                        {
                                this._UserState.Clear();
                                this._Lock.Exit();
                        }
                }
                public void SetUserState(IUserState state, Func<IUserState, IUserState, IUserState> upFun)
                {
                        SetUserStateRes result = this._SetUserState(state, false);
                        if (!result.IsSuccess)
                        {
                                IUserState newState = (IUserState)Tools.Json(result.UserState, state.GetType());
                                if (upFun != null)
                                {
                                        IUserState t = upFun(newState, state);
                                        if (t == null || t.Equals(newState))
                                        {
                                                Interlocked.Exchange(ref this._StateVer, result.StateVer);
                                                this._StateJson = result.UserState;
                                                this._Clear();
                                        }
                                        else
                                        {
                                                result = this._SetUserState(state, true);
                                        }
                                }
                        }
                        Interlocked.Exchange(ref this._StateVer, result.StateVer);
                        this._StateJson = result.UserState;
                        this._Clear();
                }
        }
}
