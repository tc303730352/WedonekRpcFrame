using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL
{
    public interface IRpcExtendResource<T> : IRepository<T> where T : class, new()
    {
    }
}