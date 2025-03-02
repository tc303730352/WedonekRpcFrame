using WeDonekRpc.Model;

namespace RpcStore.Model.Tran
{
    public class BasicTransaction
    {
        /// <summary>
        /// 事务Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 父级ID
        /// </summary>
        public long ParentId { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId { get; set; }
        /// <summary>
        /// 服务节点
        /// </summary>
        public long ServerId { get; set; }
        /// <summary>
        /// 服务节点类型
        /// </summary>
        public string SystemType { get; set; }

        /// <summary>
        /// 所属区域Id
        /// </summary>
        public int RegionId { get; set; }
        /// <summary>
        /// 事务名
        /// </summary>
        public string TranName { get; set; }

        /// <summary>
        /// 事务状态
        /// </summary>
        public TransactionStatus TranStatus { get; set; }
        /// <summary>
        /// 事务模式
        /// </summary>
        public RpcTranMode TranMode { get; set; }

        /// <summary>
        /// 是否已锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime? LockTime { get; set; }
        /// <summary>
        /// 提交状态
        /// </summary>
        public TranCommitStatus CommitStatus { get; set; }
        /// <summary>
        /// 超时时间
        /// </summary>
        public DateTime OverTime { get; set; }

        /// <summary>
        /// 事务结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 事务结束时间
        /// </summary>
        public DateTime? SubmitTime { get; set; }
        /// <summary>
        /// 事务失败时间
        /// </summary>
        public DateTime? FailTime { get; set; }
        /// <summary>
        /// 失败回滚重试次数
        /// </summary>
        public short RetryNum { get; set; }
        /// <summary>
        /// 错误码
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
    }
}
