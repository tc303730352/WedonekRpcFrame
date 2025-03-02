using RpcStore.RemoteModel.TransmitScheme.Model;

namespace RpcStore.Collect
{
    public interface ITransmitGenerateService
    {
        TransmitDatum[] GetTransmits (TransmitGenerate generate);
    }
}