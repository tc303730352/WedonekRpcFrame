using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect
{
    public interface ISyncRunStateCollect
    {
        void Sync (RunState run, long serverId);
    }
}