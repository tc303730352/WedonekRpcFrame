using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryRegion : BasicPage<Model.RegionData>
    {
    }
}
