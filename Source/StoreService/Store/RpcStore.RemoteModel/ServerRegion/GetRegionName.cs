using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetRegionName : RpcRemote<string>
    {
        public int Id { get; set; }
    }
}
