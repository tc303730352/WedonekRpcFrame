using WeDonekRpc.Model.Model;

namespace RpcCentral.DAL
{
    public interface IServerDictateLimitDAL
    {
        ServerDictateLimit[] GetDictateLimit(long serverId);
    }
}