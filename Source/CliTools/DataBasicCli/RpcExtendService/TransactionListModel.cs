using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("TransactionList", "事务日志表")]
    public class TransactionListModel
    {
        /// <summary>
        /// 事务Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "事务ID")]
        public long Id
        {
            get;
            set;
        }

        [SugarColumn(ColumnDescription = "父级事务ID")]
        public long ParentId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "服务类别值", Length = 50, ColumnDataType = "varchar")]
        public string SystemType
        {
            get;
            set;
        }

        [SugarColumn(ColumnDescription = "机房ID")]
        public int RegionId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "事务名", Length = 50, ColumnDataType = "varchar")]
        public string TranName
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "提交的数据")]
        public string SubmitJson
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "扩展参数", Length = 500)]
        public string Extend
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "事务状态")]
        public byte TranStatus
        {
            get;
            set;
        }
        /// <summary>
        /// 事务模式
        /// </summary>
        [SugarColumn(ColumnDescription = "事务模式")]
        public byte TranMode { get; set; }
        [SugarColumn(ColumnDescription = "是否是根事务")]
        public bool IsRoot { get; set; }
        [SugarColumn(ColumnDescription = "是否已锁定")]
        public bool IsLock { get; set; }
        [SugarColumn(ColumnDescription = "锁定时间")]
        public DateTime? LockTime { get; set; }

        [SugarColumn(ColumnDescription = "提交状态")]
        public byte CommitStatus { get; set; }


        /// <summary>
        ///超时时间
        /// </summary>
        [SugarColumn(ColumnDescription = "超时时间")]
        public DateTime OverTime
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "提交时间")]
        public DateTime? SubmitTime
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "失败时间")]
        public DateTime? FailTime
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "重试次数")]
        public short RetryNum
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "错误码", ColumnDataType = "varchar", Length = 100)]
        public string Error
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "结束时间")]
        public DateTime? EndTime
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }

    }
}
