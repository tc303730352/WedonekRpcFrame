using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerConfig.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerService
    {
        long Add (ServerConfigAdd add);
        void Delete (long id);
        RemoteServerDatum Get (long id);
        RemoteServerModel GetDatum (long id);
        ServerItem[] GetItems (ServerConfigQuery query);
        string GetName(long serverId);
        Dictionary<long, string> GetNames(long[] serverId);
        PagingResult<RemoteServer> Query (ServerConfigQuery query, IBasicPage paging);
        void SetService (long id, ServerConfigSet set);
        void SetServiceState (long id, RpcServiceState state);
        void SetVerNum (long id, int verNum);
    }
}