using WeDonekRpc.Client;
using RpcStore.RemoteModel.ContainerGroup.Model;
using RpcStore.RemoteModel.ServerBind.Model;

namespace RpcStore.RemoteModel.ServerBind
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetBindContainerGroup : RpcRemoteArray<ContainerGroupItem>
    {
        public BindGetParam Param { get; set; }
    }
}
