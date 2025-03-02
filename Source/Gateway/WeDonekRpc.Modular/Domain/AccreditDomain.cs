using System;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Modular.Domain
{
    internal class AccreditDomain : ILocalAccredit
    {
        private readonly IAccreditService _Service;

        public AccreditDomain(IAccreditService service)
        {
            this._Service = service;
        }
        /// <summary>
        /// 授权码
        /// </summary>
        public string AccreditId => this._Service.AccreditId;

        public void CheckAccredit()
        {
            if (this._Service.AccreditId.IsNull())
            {
                throw new ErrorException("rpc.accredit.id.null");
            }
            this._Service.CheckIsAccredit(this._Service.AccreditId);
        }
        public bool CheckIsAccredit()
        {
            if (this._Service.AccreditId.IsNull())
            {
                return false;
            }
            return this._Service.CheckIsAccredit(this._Service.AccreditId);
        }
        public IUserState GetUserState()
        {
            if (this.AccreditId.IsNotNull())
            {
                return this._Service.GetAccredit(this.AccreditId);
            }
            return null;
        }
        public void CancelAccredit()
        {
            if (this.AccreditId.IsNotNull())
            {
                this._Service.CancelAccredit(this.AccreditId);
            }
        }
    }
}
