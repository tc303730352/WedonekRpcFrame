using WeDonekRpc.SqlSugar;
using WeDonekRpc.SqlSugar.Repository;

namespace RpcExtend.DAL.Repository
{
    internal class RpcBasicRepository<T> : BasicRepository<T>, IRpcBasicRepository<T> where T : class, new()
    {
        public RpcBasicRepository (ISqlClientFactory factory) : base(factory, "RpcService")
        {
        }
    }
}
