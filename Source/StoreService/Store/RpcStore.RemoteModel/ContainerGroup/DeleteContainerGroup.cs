using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ContainerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteContainerGroup : RpcRemote
    {
        public long Id { get; set; }
    }
}
