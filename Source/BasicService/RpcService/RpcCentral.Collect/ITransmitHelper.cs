using RpcCentral.Collect.Model;

namespace RpcCentral.Collect
{
    public interface ITransmitHelper
    {
        Transmit[] GetTransmits (long rpcMerId, long systemType);
    }
}