using RpcStore.Model.DB;
using WeDonekRpc.Model.Model;

namespace RpcStore.DAL
{
    public interface IServerClientLimitDAL
    {
        long Add (ServerClientLimitModel add);
        void Clear (long serverId);
        void Clear (long rpcMerId, long serverId);
        void Delete (long id);
        ServerClientLimitModel Get (long id);
        ServerClientLimitModel Get (long rpcMerId, long serverId);
        ServerClientLimitModel[] Gets (long[] rpcMerId, long serverId);
        void Set (long id, ServerClientLimit config);
    }
}