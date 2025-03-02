using SqlSugar;
using WeDonekRpc.Model;

namespace RpcSync.Model.DB
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
        /// 事务模式
        /// </summary>
        public RpcTranMode TranMode { get; set; }
        /// <summary>
        /// 是否为根事务
        /// </summary>
        public bool IsRoot { get; set; }
        /// <summary>
        /// 是否锁定
        /// </summary>
        public bool IsLock { get; set; }
        /// <summary>
        /// 锁定时间
        /// </summary>
        public DateTime LockTime { get; set; }
        /// <summary>
        /// 事务提交状态
        /// </summary>
        public TranCommitStatus CommitStatus { get; set; }
        /// <summary>
        ///超时时间
        /// </summary>
        public DateTime OverTime
        {
            get;
            set;
        }
        /// <summary>
        /// 提交时间
        /// </summary>
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
        /// <summary>
        /// 重试数
        /// </summary>
        public short RetryNum
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get;
            set;
        }
        /// <summary>
        /// 事务结束时间
        /// </summary>
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
