using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CheckRegionName : RpcRemote
    {
        public string RegionName { get; set; }
    }
}
