using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerPublic.Model;

namespace RpcStore.RemoteModel.ServerPublic
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryPublicScheme : BasicPage<ServerPublicScheme>
    {
        public PublicSchemeQuery Query { get; set; }
    }
}
