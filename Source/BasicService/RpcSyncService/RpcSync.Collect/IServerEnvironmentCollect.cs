using WeDonekRpc.Model.Server;

namespace RpcSync.Collect
{
    public interface IServerEnvironmentCollect
    {
        void SetModules (long serverId, ProcModule[] modules);
        void Sync (long serverId, EnvironmentConfig config);
    }
}