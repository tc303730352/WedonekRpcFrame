using RpcStore.Model.DB;

namespace RpcStore.Collect
{
    public interface IServerSignalStateCollect
    {
        ServerSignalStateModel[] Gets(long serverId);
    }
}