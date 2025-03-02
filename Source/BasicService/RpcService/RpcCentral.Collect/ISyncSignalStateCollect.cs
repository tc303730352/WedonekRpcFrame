using WeDonekRpc.Model.Server;

namespace RpcCentral.Collect
{
    public interface ISyncSignalStateCollect
    {
        void Sync(long serverId, RemoteState[] remotes);
    }
}