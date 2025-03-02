using RpcCentral.Model.DB;

namespace RpcCentral.DAL
{
    public interface IServerSignalStateDAL
    {
        bool SyncState(ServerSignalState[] states);
    }
}