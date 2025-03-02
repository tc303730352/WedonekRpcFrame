using WeDonekRpc.Client;
using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class SetTransmitScheme : RpcRemote
    {
        public long Id { get; set; }
        public TransmitSchemeSet Datum { get; set; }
    }
}
