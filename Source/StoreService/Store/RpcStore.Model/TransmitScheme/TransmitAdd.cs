using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.Model.TransmitScheme
{
    public class TransmitAdd : TransmitSchemeAdd
    {
        public TransmitDatum[] Transmit { get; set; }
    }
}
