using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddRegion : RpcRemote<int>
    {
        public RegionDatum Datum { get; set; }
    }
}
