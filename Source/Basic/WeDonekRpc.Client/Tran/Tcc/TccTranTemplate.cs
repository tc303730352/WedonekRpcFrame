using System;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Tran.Tcc
{
    /// <summary>
    /// Tcc事务摸版
    /// </summary>
    internal class TccTranTemplate : ITranTemplate
    {
        private readonly IIocService _Unity = null;
        public TccTranTemplate (string name)
        {
            this.TranMode = RpcTranMode.Tcc;
            this.TranName = name;
            this._Unity = RpcClient.Ioc;
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
        } = RpcTranMode.Tcc;
        public IDisposable BeginTran (ICurTran tran)
        {
            return null;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="tran">事务</param>
        public void Commit (ICurTran tran)
        {
            IRpcTccEvent action = this._Unity.Resolve<IRpcTccEvent>(this.TranName);
            action.Commit(tran);
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="tran">事务</param>
        public void Rollback (ICurTran tran)
        {
            IRpcTccEvent action = this._Unity.Resolve<IRpcTccEvent>(this.TranName);
            action.Rollback(tran);
        }
    }
}
