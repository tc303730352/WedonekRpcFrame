using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IServerCurConfigDAL
    {
        void Add (ServerCurConfigModel add);
        bool IsExists (long serverId);
        void Set (ServerCurConfigModel set);
    }
}