using WeDonekRpc.Client;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Identity.Model;

namespace WeDonekRpc.ModularModel.Identity
{
    [IRemoteConfig("sys.extend")]
    public class GetIdentity : RpcRemote<IdentityDatum>
    {
        public string IdentityId
        {
            get;
            set;
        }
    }
}
