using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.Collect
{
    public interface IServerDictateLimitCollect
    {
        long Add(DictateLimitAdd add);
        void Delete(ServerDictateLimitModel limit);
        ServerDictateLimitModel Get(long id);
        ServerDictateLimitModel[] Query(DictateLimitQuery query, IBasicPage paging, out int count);
        bool SetDictateLimit(ServerDictateLimitModel source, DictateLimitSet limit);
    }
}