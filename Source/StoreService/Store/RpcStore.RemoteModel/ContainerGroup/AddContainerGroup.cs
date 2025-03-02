using WeDonekRpc.Client;
using RpcStore.RemoteModel.ContainerGroup.Model;

namespace RpcStore.RemoteModel.ContainerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddContainerGroup : RpcRemote<int>
    {
        public ContainerGroupAdd Datum { get; set; }
    }
}
