using WeDonekRpc.Model.Model;

namespace RpcCentral.DAL
{
    public interface IServerLimitConfigDAL
    {
        ServerLimitConfig GetLimitConfig(long serverId);
    }
}