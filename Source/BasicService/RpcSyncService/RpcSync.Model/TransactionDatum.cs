using WeDonekRpc.Model;

namespace RpcSync.Model
{
    /// <summary>
    /// 事务资料
    /// </summary>
    [Serializable]
    public class TransactionDatum
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 事务父级ID
        /// </summary>
        public long ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 区域ID
        /// </summary>
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 事务名
        /// </summary>
        public string TranName
        {
            get;
            set;
        }
        /// <summary>
        /// 提交的JSON数据
        /// </summary>
        public string SubmitJson
        {
            get;
            set;
        }
        /// <summary>
        /// 扩展参数
        /// </summary>
        public string Extend
        {
            get;
            set;
        }
        /// <summary>
        /// 事务状态
        /// </summary>
        public TransactionStatus TranStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 提交状态
        /// </summary>
        public TranCommitStatus CommitStatus
        {
            get;
            set;
        }
        /// <summary>
        ///超时时间
        /// </summary>
        public DateTime OverTime
        {
            get;
            set;
        }
        /// <summary>
        /// 事务模式
        /// </summary>
        public RpcTranMode TranMode { get; set; }
        /// <summary>
        /// 重试数
        /// </summary>
        public short RetryNum { get; set; }

        /// <summary>
        /// 错误ID
        /// </summary>
        public string Error { get; set; }
    }
}
