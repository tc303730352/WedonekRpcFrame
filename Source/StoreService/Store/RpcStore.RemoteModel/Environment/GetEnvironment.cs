using WeDonekRpc.Client;
using RpcStore.RemoteModel.Environment.Model;

namespace RpcStore.RemoteModel.Environment
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetEnvironment : RpcRemote<ServerEnvironment>
    {
        public long ServerId { get; set; }
    }
}
