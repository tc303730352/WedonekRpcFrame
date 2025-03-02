using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect
{
    public interface IServerClientLimitCollect
    {
        ServerClientLimit GetClientLimit (long rpcMerId, long serverId);
        void Refresh (long rpcMerId, long serverId);
    }
}