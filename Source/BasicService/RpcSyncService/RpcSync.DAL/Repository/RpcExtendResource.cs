using WeDonekRpc.SqlSugar;
using WeDonekRpc.SqlSugar.Repository;

namespace RpcSync.DAL.Repository
{
    internal class RpcExtendResource<T> : BasicRepository<T>, IRpcExtendResource<T> where T : class, new()
    {
        public RpcExtendResource (ISqlClientFactory factory) : base(factory, "RpcExtend")
        {
        }
    }
}
