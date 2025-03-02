using WeDonekRpc.Model.Model;

namespace RpcCentral.Collect
{
    public interface IServerDictateLimitCollect
    {
        ServerDictateLimit[] GetDictateLimit(long serverId);
        void Refresh(long serverId);
    }
}