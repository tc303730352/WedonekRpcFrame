using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL
{
    public interface IRpcBasicRepository<T> : IRepository<T> where T : class, new()
    {
    }
}
