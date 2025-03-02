using System;

namespace WeDonekRpc.Model.Tran.Model
{
    /// <summary>
    /// 事务状态
    /// </summary>
    public class RpcTranState
    {
        /// <summary>
        /// 事务当前状态
        /// </summary>
        public TransactionStatus TranStatus { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 设定的过期时间
        /// </summary>
        public DateTime OverTime { get; set; }
    }
}
