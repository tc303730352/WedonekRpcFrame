using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class DeleteTransmitScheme : RpcRemote
    {
        public long Id { get; set; }
    }
}
