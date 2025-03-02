using WeDonekRpc.Client;
using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class AddTransmitScheme : RpcRemote<long>
    {
        public TransmitSchemeAdd Datum { get; set; }
    }
}
