using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.DictateLimit.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerDictateLimitService
    {
        long Add(DictateLimitAdd add);
        void Delete(long id);
        DictateLimit Get(long id);
        PagingResult<DictateLimit> Query(DictateLimitQuery query, IBasicPage paging);
        void Set(long id, DictateLimitSet set);
    }
}