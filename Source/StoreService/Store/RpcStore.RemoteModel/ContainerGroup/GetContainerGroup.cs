using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ContainerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetContainerGroup : RpcRemote<Model.ContainerGroupDatum>
    {
        public long Id { get; set; }
    }
}
