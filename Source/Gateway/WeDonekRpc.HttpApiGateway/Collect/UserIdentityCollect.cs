using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.Modular;

namespace WeDonekRpc.HttpApiGateway.Collect
{
    /// <summary>
    /// 用户身份标识集合
    /// </summary>
    internal class UserIdentityCollect : IUserIdentityCollect
    {
        private readonly IIdentityService _Identity;
        private readonly IGatewayIdentityConfig _Config;
        public UserIdentityCollect (IIdentityService service, IGatewayIdentityConfig config)
        {
            this._Config = config;
            this._Identity = service;
        }
        public bool IsEnableIdentity => this._Identity.IsEnableIdentity;

        public void CheckIdentity (string identityId)
        {
            if (!identityId.IsNull())
            {
                this._Identity.IdentityId = identityId;
            }
            this._Identity.CheckState();
        }
        public void ClearIdentity ()
        {
            this._Identity.Clear();
        }
        public void InitIdentity (IApiService service)
        {
            if (!this._Identity.IsEnableIdentity || this._Config.ReadMode == ApiGateway.Config.IdentityReadMode.无)
            {
                return;
            }
            else if (this._Config.ReadMode == ApiGateway.Config.IdentityReadMode.Head)
            {
                this._Identity.IdentityId = service.Request.Headers[this._Config.ParamName];
            }
            else if (this._Config.ReadMode == ApiGateway.Config.IdentityReadMode.GET)
            {
                this._Identity.IdentityId = service.Request.QueryString[this._Config.ParamName];
            }
            else if (!service.AccreditId.IsNull())
            {
                this._Identity.IdentityId = service.UserState.GetValue<string>(this._Config.ParamName);
            }
        }

    }
}
