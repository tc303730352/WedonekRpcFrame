using SqlSugar;

namespace WeDonekRpc.SqlSugar
{
    public interface ILocalTran : IDisposable
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        long TranId { get; }
        /// <summary>
        /// 开始事务
        /// </summary>
        void BeginTran ();
        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran ();
        /// <summary>
        /// 获取Provider对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        SqlSugarProvider GetProvider (string name);
        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran ();
        /// <summary>
        /// 挂起事务
        /// </summary>
        void PendingTran ();
    }
}