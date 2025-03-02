using System;

namespace WeDonekRpc.Model.Tran.Model
{
    /// <summary>
    /// 事务结果
    /// </summary>
    public class TranResult
    {
        /// <summary>
        /// 事务当前状态
        /// </summary>
        public TransactionStatus TranStatus { get; set; }
        /// <summary>
        /// 日志列表
        /// </summary>
        public TranLog[] Logs { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }
    }
}
