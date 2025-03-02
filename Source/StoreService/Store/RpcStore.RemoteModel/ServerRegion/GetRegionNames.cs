using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetRegionNames : RpcRemote<Dictionary<int, string>>
    {
        public int[] Ids { get; set; }
    }
}
