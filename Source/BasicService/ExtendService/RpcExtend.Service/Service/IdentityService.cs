using RpcExtend.Collect;
using RpcExtend.Model;
using RpcExtend.Service.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.Identity.Model;
using WeDonekRpc.ModularModel.Identity.Msg;

namespace RpcExtend.Service.Service
{
    internal class IdentityService : IIdentityService
    {
        private readonly IIdentityAppCollect _IdentityApp;
        public IdentityService (IIdentityAppCollect identityApp)
        {
            this._IdentityApp = identityApp;
        }
        public void Refresh (RefreshIdentity identity)
        {
            this._IdentityApp.Refresh(identity.AppId);
            new UpdateIdentity { AppId = identity.AppId }.Send();
        }
        public IdentityDatum GetIdentity (string id)
        {
            IdentityApp app = this._IdentityApp.GetByAppId(id);
            if (!app.IsEnable || ( app.EffectiveDate.HasValue && app.EffectiveDate.Value < HeartbeatTimeHelper.CurrentDate ))
            {
                return new IdentityDatum
                {
                    IsValid = false
                };
            }
            return new IdentityDatum
            {
                IsValid = true,
                AppName = app.AppName,
                AppExtend = app.AppExtend
            };
        }
    }
}
