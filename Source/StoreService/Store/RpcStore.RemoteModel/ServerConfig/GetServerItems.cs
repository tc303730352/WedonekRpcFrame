using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.RemoteModel.ServerConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerItems : RpcRemoteArray<ServerItem>
    {
        public Model.ServerConfigQuery Query { get; set; }
    }
}
