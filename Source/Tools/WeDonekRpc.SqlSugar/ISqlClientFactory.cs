using SqlSugar;

namespace WeDonekRpc.SqlSugar
{
    public interface ISqlClientFactory : IDisposable
    {
        ILocalTran Current { get; }
        SqlSugarProvider GetProvider (string name);
        bool IsTran { get; }
        void BeginTran ();
        void CommitTran (ILocalTran source);

        void RollbackTran (ILocalTran source);
        ISugarQueryable<T> GetQueryable<T> (string name);
    }
}