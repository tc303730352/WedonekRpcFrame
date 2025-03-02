using RpcStore.Model.DB;

namespace RpcStore.DAL
{
    public interface IServerCurConfigDAL
    {
        void Delete (long serverId);
        ServerCurConfigModel GetConfig (long serverId);
    }
}