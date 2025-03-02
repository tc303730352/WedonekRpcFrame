using WeDonekRpc.Model.Model;

namespace RpcCentral.DAL
{
    public interface IServerClientLimitDAL
    {
        ServerClientLimit GetClientLimit(long rpcMerId, long serverId);
    }
}