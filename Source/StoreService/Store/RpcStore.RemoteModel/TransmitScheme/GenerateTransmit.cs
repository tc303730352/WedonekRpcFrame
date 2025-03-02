using RpcStore.RemoteModel.TransmitScheme.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.TransmitScheme
{
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GenerateTransmit : RpcRemoteArray<TransmitDatum>
    {
        public TransmitGenerate Arg
        {
            get;
            set;
        }
    }
}
