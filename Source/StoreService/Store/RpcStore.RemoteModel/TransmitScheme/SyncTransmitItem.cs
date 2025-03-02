using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SyncTransmitItem : RpcRemote
    {
        public long SchemeId { get; set; }

        public TransmitDatum[] Transmits { get; set; }
    }
}
