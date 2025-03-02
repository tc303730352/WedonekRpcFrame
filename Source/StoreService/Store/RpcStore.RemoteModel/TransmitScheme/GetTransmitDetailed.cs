using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetTransmitDetailed : RpcRemote<TransmitSchemeDetailed>
    {
        public long Id { get; set; }
    }
}
