using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.RemoteModel.ServerConfig
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetServerDatum : RpcRemote<RemoteServerModel>
    {
        public long Id { get; set; }
    }
}
