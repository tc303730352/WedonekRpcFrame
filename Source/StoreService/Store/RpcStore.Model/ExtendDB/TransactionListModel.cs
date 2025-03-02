using SqlSugar;
using WeDonekRpc.Model;

namespace RpcStore.Model.ExtendDB
{
    [SugarTable("TransactionList")]
    public class TransactionListModel
    {
        /// <summary>
        /// 事务Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 父级事务Id
        /// </summary>
        public long ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类别
        /// </summary>
        public string SystemType
        {
            get;
            set;
        }

        /// <summary>
        /// 区域Id
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
        /// 提交的数据
        /// </summary>
        public string SubmitJson
        {
            get;
            set;
        }
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
        /// 事务模式
        /// </summary>
        public RpcTranMode TranMode { get; set; }

        public bool IsRoot { get; set; }

        public bool IsLock { get; set; }

        public DateTime LockTime { get; set; }

        public TranCommitStatus CommitStatus { get; set; }
        /// <summary>
        ///超时时间
        /// </summary>
        public DateTime OverTime
        {
            get;
            set;
        }
        public DateTime? SubmitTime
        {
            get;
            set;
        }
        /// <summary>
        /// 失败时间
        /// </summary>
        public DateTime? FailTime
        {
            get;
            set;
        }
        public short RetryNum
        {
            get;
            set;
        }
        public string Error
        {
            get;
            set;
        }
        public DateTime? EndTime
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get;
            set;
        }

    }
}
