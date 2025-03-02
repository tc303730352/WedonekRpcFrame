using System;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Tran.Saga
{
    /// <summary>
    /// Saga事务模板
    /// </summary>
    internal class SagaTranTemplate : ITranTemplate
    {
        private readonly Action<ICurTran> _Rollback = null;
        public SagaTranTemplate (string name, Action<ICurTran> action)
        {
            this.TranMode = RpcTranMode.Saga;
            this.TranName = name;
            this._Rollback = action;
        }
        /// <summary>
        /// 事务名
        /// </summary>
        public string TranName
        {
            get;
        }
        /// <summary>
        /// 事务执行方式
        /// </summary>
        public RpcTranMode TranMode
        {
            get;
            set;
        }

        public IDisposable BeginTran (ICurTran tran)
        {
            return null;
        }

        public void Commit (ICurTran cur)
        {

        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="cur">当前事务</param>
        /// <param name="extend">扩展数据</param>
        public void Rollback (ICurTran cur)
        {
            this._Rollback(cur);
        }
    }

}
