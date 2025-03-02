using RpcCentral.Model;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect
{
    public interface IRpcServerStateCollect
    {
        RunEnvironment GetEnvironments(long serverId);
        void SyncRunState(long id, ProcessDatum datum);
    }
}