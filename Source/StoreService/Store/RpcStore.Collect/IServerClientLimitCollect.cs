using RpcStore.Model.DB;
using RpcStore.RemoteModel.ClientLimit.Model;

namespace RpcStore.Collect
{
    public interface IServerClientLimitCollect
    {
        void Delete(ServerClientLimitModel config);
        ServerClientLimitModel Get(long id);
        ServerClientLimitModel Get(long rpcMerId, long serverId);
        ServerClientLimitModel[] Gets (long[] rpcMerId, long serverId);
        bool Sync(ClientLimitDatum add);
    }
}