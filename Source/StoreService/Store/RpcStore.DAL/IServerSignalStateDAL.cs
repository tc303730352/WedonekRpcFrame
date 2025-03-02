using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServerSignalStateDAL
    {
        void Clear(long serverId);
        void ClearRemote(long remoteId);
        ServerSignalStateModel[] Gets(long serverId);
    }
}