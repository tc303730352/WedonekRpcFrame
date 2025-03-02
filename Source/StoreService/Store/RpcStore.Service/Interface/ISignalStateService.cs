using RpcStore.RemoteModel.SignalState.Model;

namespace RpcStore.Service.Interface
{
    public interface ISignalStateService
    {
        ServerSignalState[] Gets(long serverId);
    }
}