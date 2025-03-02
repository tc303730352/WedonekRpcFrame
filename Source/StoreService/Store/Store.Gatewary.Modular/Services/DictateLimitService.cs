using WeDonekRpc.Model;
using RpcStore.RemoteModel.DictateLimit;
using RpcStore.RemoteModel.DictateLimit.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class DictateLimitService : IDictateLimitService
    {
        public long AddDictateLimit(DictateLimitAdd datum)
        {
            return new AddDictateLimit
            {
                Datum = datum,
            }.Send();
        }

        public void DeleteDictateLimit(long id)
        {
            new DeleteDictateLimit
            {
                Id = id,
            }.Send();
        }

        public DictateLimit GetDictateLimit(long id)
        {
            return new GetDictateLimit
            {
                Id = id,
            }.Send();
        }

        public DictateLimit[] QueryDictateLimit(DictateLimitQuery query, IBasicPage paging, out int count)
        {
            return new QueryDictateLimit
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetDictateLimit(long id, DictateLimitSet datum)
        {
            new SetDictateLimit
            {
                Id = id,
                Datum = datum,
            }.Send();
        }

    }
}
