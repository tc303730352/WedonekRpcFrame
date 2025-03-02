using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.DAL
{
    public interface IServerDictateLimitDAL
    {
        long Add(ServerDictateLimitModel add);
        bool CheckIsExists(long serverId, string dictate);
        void Clear(long serverId);
        void Delete(long id);
        ServerDictateLimitModel Get(long id);
        ServerDictateLimitModel[] Query(DictateLimitQuery query, IBasicPage paging, out int count);
        void Set(long id, DictateLimitSet limit);
    }
}