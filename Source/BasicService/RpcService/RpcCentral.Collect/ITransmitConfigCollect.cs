using RpcCentral.Collect.Controller;

namespace RpcCentral.Collect
{
    public interface ITransmitConfigCollect
    {
        TransmitConfigController GetTransmit ( long systemType, long rpcMerId );
        void Refresh ( long systemType, long rpcMerId );
    }
}