using RpcCentral.Model;

namespace RpcCentral.DAL
{
    public interface IServerTransmitConfigDAL
    {
        ServerTransmitScheme[] Gets (long systemType, long rpcMerId);
    }
}