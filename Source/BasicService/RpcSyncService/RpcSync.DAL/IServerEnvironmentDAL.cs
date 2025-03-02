using WeDonekRpc.Model.Server;
using RpcSync.Model.DB;

namespace RpcSync.DAL
{
    public interface IServerEnvironmentDAL
    {
        void SetModules (long id, ProcModule[] modules);
        void Add (long serverId, EnvironmentConfig config);
        ServerEnvironmentModel Get (long serverId);
        long GetId (long serverId);
        bool Set (ServerEnvironmentModel model, EnvironmentConfig set);
    }
}