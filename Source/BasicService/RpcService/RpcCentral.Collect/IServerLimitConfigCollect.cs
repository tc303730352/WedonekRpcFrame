using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect
{
    public interface IServerLimitConfigCollect
    {
        ServerLimitConfig GetServerLimit(long serverId);
        void Refresh(long serverId);
    }
}