using WeDonekRpc.Client.Attr;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Modular.Model
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RpcUserIdentity : IUserIdentity
    {
        private readonly IIdentityService _Service = null;
        public RpcUserIdentity (IIdentityService service)
        {
            this._Service = service;
        }
        public string IdentityId => this._Service.IdentityId;

        /// <summary>
        /// 是否启用身份标识
        /// </summary>
        public bool IsEnableIdentity => this._Service.IsEnableIdentity;

        public void SetIdentityId (string identityId)
        {
            if (!this._Service.IsEnableIdentity)
            {
                throw new ErrorException("rpc.identity.no.enable");
            }
            this._Service.SetIdentityId(identityId);
        }
        /// <summary>
        /// 获取用户身份标识领域
        /// </summary>
        /// <returns></returns>
        public UserIdentity GetIdentity ()
        {
            if (!this._Service.IsEnableIdentity)
            {
                throw new ErrorException("rpc.identity.no.enable");
            }
            else if (this._Service.IdentityId.IsNull())
            {
                throw new ErrorException("rpc.identity.id.null");
            }
            return this._Service.GetIdentity();
        }


    }
}
