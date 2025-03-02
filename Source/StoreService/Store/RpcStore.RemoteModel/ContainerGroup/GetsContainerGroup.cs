using WeDonekRpc.Client;
using RpcStore.RemoteModel.ContainerGroup.Model;

namespace RpcStore.RemoteModel.ContainerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetsContainerGroup : RpcRemoteArray<ContainerGroupItem>
    {
        public int? RegionId { get; set; }
    }
}
