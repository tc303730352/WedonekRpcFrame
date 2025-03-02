using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DropRegion : RpcRemote
    {
        public int Id { get; set; }
    }
}
