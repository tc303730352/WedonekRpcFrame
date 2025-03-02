using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.RemoteModel.ServerPublic
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddPublicScheme : RpcRemote<long>
    {
        public PublicSchemeAdd SchemeAdd { get; set; }
    }
}
