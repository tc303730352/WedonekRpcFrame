using WeDonekRpc.Client;
using RpcStore.RemoteModel.ContainerGroup.Model;

namespace RpcStore.RemoteModel.ContainerGroup
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetContainerGroup : RpcRemote
    {
        public long Id
        {
            get;
            set;
        }
        public ContainerGroupSet Datum
        {
            get;
            set;
        }
    }
}
