using WeDonekRpc.ModularModel.Identity.Model;
using WeDonekRpc.ModularModel.Identity.Msg;

namespace RpcExtend.Service.Interface
{
    public interface IIdentityService
    {
        void Refresh (RefreshIdentity identity);
        IdentityDatum GetIdentity (string id);
    }
}