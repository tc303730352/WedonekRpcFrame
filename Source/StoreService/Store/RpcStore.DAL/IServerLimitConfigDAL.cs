using WeDonekRpc.Model.Model;
using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServerLimitConfigDAL
    {
        void Add(ServerLimitConfigModel add);
        void Delete(long serverId);
        ServerLimitConfigModel Get(long serverId);
        void Set(long serverId, ServerLimitConfig config);
    }
}