using WeDonekRpc.Client;
using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryTransmitScheme : BasicPage<Model.TransmitScheme>
    {
        public TransmitSchemeQuery Query { get; set; }
    }
}
