using System;
using System.Threading;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Modular.Domain;
using WeDonekRpc.Modular.Model;

namespace WeDonekRpc.Modular.Accredit
{
    /// <summary>
    /// 默认授权服务
    /// </summary>
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class DefAccreditService : AccreditService, IAccreditService, IAccredit
    {
        public override void ClearAccredit ()
        {
            _UserState.Value = null;
            base.ClearAccredit();
        }

        public DefAccreditService ()
        {

        }
        private static readonly AsyncLocal<IUserState> _UserState = new AsyncLocal<IUserState>(_StateChange);

        private static void _StateChange ( AsyncLocalValueChangedArgs<IUserState> e )
        {
            if ( e.CurrentValue != null && e.CurrentValue.AccreditId != AccreditService._CurAccreditId )
            {
                _UserState.Value = null;
            }
        }

        private static readonly Type _StateType = typeof(UserState);
        public IUserState CurrentUser => _UserState.Value;


        public IUserState GetAccredit ( string accreditId )
        {
            UserAccreditDomain accredit = _GetAccredit(accreditId);
            _UserState.Value = accredit.GetUserState(_StateType);
            return _UserState.Value;
        }
        public void CancelAccredit ( string accreditId )
        {
            Cancel(accreditId);
        }
        /// <summary>
        /// 设置当前授权并检查授权信息
        /// </summary>
        /// <param name="accreditId"></param>
        /// <returns></returns>
        public IUserState SetCurrentAccredit ( string accreditId )
        {
            UserAccreditDomain accredit = base.SetAccredit(accreditId);
            _UserState.Value = accredit.GetUserState(_StateType);
            return _UserState.Value;
        }
    }
}
