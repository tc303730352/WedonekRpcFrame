using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ContainerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryContainerGroup : BasicPage<Model.ContainerGroup>
    {
        public Model.ContainerGroupQuery Query { get; set; }
    }
}
