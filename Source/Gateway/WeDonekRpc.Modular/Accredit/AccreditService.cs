using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Model;
using WeDonekRpc.Modular.Config;
using WeDonekRpc.Modular.Domain;
using WeDonekRpc.Modular.Model;
using WeDonekRpc.ModularModel.Accredit;
using WeDonekRpc.ModularModel.Accredit.Model;
using WeDonekRpc.ModularModel.Accredit.Msg;

namespace WeDonekRpc.Modular.Accredit
{
    /// <summary>
    /// 授权模块
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class AccreditService : IAccreditController
    {
        private static readonly ConcurrentDictionary<string, UserAccreditDomain> _AccreditList = new ConcurrentDictionary<string, UserAccreditDomain>();

        /// <summary>
        /// 当前请求的AccreditId
        /// </summary>
        private static readonly AsyncLocal<string> _AccreditId = new AsyncLocal<string>();
        private static readonly Timer _ClearAccredit;
        public string AccreditId => _AccreditId.Value;

        static AccreditService ()
        {
            RpcClient.Route.AddRoute<AccreditRefresh>("AccreditRefresh", _AccreditRefresh);
            int time = Tools.GetRandom(9000, 11000);
            _ClearAccredit = new Timer(_Clear, null, time, time);
        }
        public AccreditService ()
        {

        }

        public virtual void ClearAccredit ()
        {
            _AccreditId.Value = null;
        }
        public void SetAccreditId (string accreditId)
        {
            _AccreditId.Value = accreditId;
        }
        /// <summary>
        /// 
        /// </summary>
        private static void _Clear (object state)
        {
            if (_AccreditList.IsEmpty)
            {
                return;
            }
            string[] keys = _AccreditList.Keys.ToArray();
            int time = HeartbeatTimeHelper.HeartbeatTime;
            keys.ForEach(a =>
            {
                if (_AccreditList.TryGetValue(a, out UserAccreditDomain data))
                {
                    if (!data.CheckStatus(time))
                    {
                        _ = _AccreditList.TryRemove(a, out data);
                    }
                }
            });
        }

        private static void _AccreditRefresh (AccreditRefresh obj)
        {
            _ = _AccreditList.TryRemove(obj.AccreditId, out _);
        }

        public void KickOutAccredit (string applyId)
        {
            new KickOutAccredit
            {
                CheckKey = applyId
            }.Send();
        }
        /// <summary>
        /// 申请临时授权
        /// </summary>
        /// <param name="isInherit">是否继承</param>
        /// <param name="expire">授权过期时间(秒)</param>
        /// <returns>授权码</returns>
        public string ApplyTempAccredit (bool isInherit, int? expire = null)
        {
            if (!this.AccreditId.IsNull())
            {
                throw new ErrorException("rpc.accredit.null");
            }
            this.CheckAccredit(this.AccreditId);
            ApplyAccreditRes res = new ApplyAccredit
            {
                ParentId = this.AccreditId,
                AccreditId = Guid.NewGuid().ToString("N"),
                IsInherit = isInherit,
                Expire = expire
            }.Send();
            return _AddAccredit(res.Accredit, null, res.StateVer);
        }
        /// <summary>
        /// 新增临时授权
        /// </summary>
        /// <param name="applyId">用户身份标识(唯一)</param>
        /// <param name="state">状态值</param>
        /// <param name="isInherit">是否继承父授权的角色和状态值</param>
        /// <param name="expire">授权过期时间</param>
        /// <returns>授权码</returns>
        public string ApplyTempAccredit (string applyId, UserState state, bool isInherit, int? expire = null)
        {
            if (!this.AccreditId.IsNull())
            {
                throw new ErrorException("rpc.accredit.null");
            }
            this.CheckAccredit(this.AccreditId);
            ApplyAccreditRes res = new ApplyAccredit
            {
                ParentId = this.AccreditId,
                AccreditId = Guid.NewGuid().ToString("N"),
                State = state.ToJson(),
                ApplyId = applyId,
                IsInherit = isInherit,
                Expire = expire
            }.Send();
            return _AddAccredit(res.Accredit, null, res.StateVer);
        }
        /// <summary>
        /// 新增授权
        /// </summary>
        /// <param name="applyId">用户身份标识(唯一)，不单点登陆传空</param>
        /// <param name="state">状态值(存储了权限和业务参数)</param>
        /// <param name="expire">授权过期时间(秒)</param>
        /// <returns></returns>
        public string AddAccredit (string applyId, UserState state, int? expire = null)
        {
            MsgSource source = RpcClient.CurrentSource;
            string json = state == null ? null : JsonTools.Json(state);
            ApplyAccreditRes res = new ApplyAccredit
            {
                Expire = expire,
                ApplyId = applyId,
                RoleType = ModularConfig.AccreditRoleType,
                AccreditId = Guid.NewGuid().ToString("N"),
                State = json
            }.Send();
            state.BindAccreditId(res.Accredit.AccreditId, RpcClient.GroupTypeVal, source.RpcMerId);
            return _AddAccredit(res.Accredit, json, res.StateVer);
        }

        private static string _AddAccredit (AccreditRes res, string state, int stateVer)
        {
            UserAccreditDomain accredit = new UserAccreditDomain(stateVer, res, state);
            accredit = _AccreditList.GetOrAdd(accredit.AccreditId, accredit);
            return accredit.AccreditId;
        }
        private static UserAccreditDomain _AddAccredit (AccreditDatum datum)
        {
            UserAccreditDomain accredit = new UserAccreditDomain(datum);
            accredit = _AccreditList.GetOrAdd(accredit.AccreditId, accredit);
            return accredit;
        }
        protected static UserAccreditDomain _GetAccredit (string accreditId)
        {
            if (!_AccreditList.TryGetValue(accreditId, out UserAccreditDomain accredit))
            {
                AccreditDatum res = new GetAccredit
                {
                    AccreditId = accreditId
                }.Send();
                return _AddAccredit(res);
            }
            else if (accredit.IsError)
            {
                throw new ErrorException(accredit.Error);
            }
            accredit.Refresh();
            return accredit;
        }

        public void ToUpdate (string accreditId, UserState state, int? expire = null)
        {
            UserAccreditDomain accredit = _GetAccredit(accreditId);
            accredit.ToUpdate(state, expire);
        }

        public UserAccreditDomain SetAccredit (string accreditId)
        {
            UserAccreditDomain accredit = _GetAccredit(accreditId);
            _AccreditId.Value = accredit.AccreditId;
            return accredit;
        }
        public static void Cancel (string accreditId)
        {
            UserAccreditDomain accredit = _GetAccredit(accreditId);
            accredit.CancelAccredit();
        }

        internal static bool SetUserState (string accreditId, IUserState state)
        {
            UserAccreditDomain accredit = _GetAccredit(accreditId);
            return accredit.SetUserState(state);
        }

        internal static IUserState SetUserState (string accreditId, UserState state, Func<IUserState, IUserState, IUserState> upFun)
        {
            UserAccreditDomain accredit = _GetAccredit(accreditId);
            if (accredit.SetUserState(state, upFun))
            {
                return accredit.GetUserState(state.GetType());
            }
            return null;
        }

        public void CheckAccredit (string accreditId)
        {
            if (!_AccreditList.TryGetValue(accreditId, out UserAccreditDomain accredit))
            {
                AccreditDatum res = new GetAccredit
                {
                    AccreditId = accreditId
                }.Send();
                accredit = _AddAccredit(res);
            }
            if (accredit.IsError)
            {
                throw new ErrorException(accredit.Error);
            }
        }
        public bool CheckIsAccredit (string accreditId)
        {
            if (!_AccreditList.TryGetValue(accreditId, out UserAccreditDomain accredit))
            {
                AccreditDatum res = new GetAccredit
                {
                    AccreditId = accreditId
                }.Send();
                accredit = _AddAccredit(res);
            }
            return !accredit.IsError;
        }
    }
}
