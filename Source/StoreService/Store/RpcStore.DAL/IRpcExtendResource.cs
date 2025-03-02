using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL
{
    public interface IRpcExtendResource<T> : IRepository<T> where T : class, new()
    {
    }
}