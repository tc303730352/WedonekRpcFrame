using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.ServerEventSwitch
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteEventSwitch : RpcRemote
    {
        public long Id { get; set; }
    }
}
