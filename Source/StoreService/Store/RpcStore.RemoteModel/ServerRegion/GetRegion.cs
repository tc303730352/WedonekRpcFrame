using WeDonekRpc.Client;
using RpcStore.RemoteModel.ServerRegion.Model;

namespace RpcStore.RemoteModel.ServerRegion
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetRegion : RpcRemote<RegionDto>
    {
        public int Id { get; set; }
    }
}
