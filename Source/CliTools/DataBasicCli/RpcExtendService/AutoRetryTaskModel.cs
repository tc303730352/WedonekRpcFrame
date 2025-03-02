using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("AutoRetryTask")]
    public class AutoRetryTaskModel
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "任务ID")]
        public long Id { get; set; }
        /// <summary>
        /// 添加任务的集群ID
        /// </summary>
        [SugarColumn(ColumnDescription = "添加任务的集群ID")]
        public long RpcMerId { get; set; }
        /// <summary>
        /// 所在机房
        /// </summary>
        [SugarColumn(ColumnDescription = "所在机房")]
        public int RegionId { get; set; }
        /// <summary>
        /// 添加任务的节点
        /// </summary>
        [SugarColumn(ColumnDescription = "添加任务的节点")]
        public long ServerId { get; set; }

        /// <summary>
        /// 标识ID
        /// </summary>
        [SugarColumn(ColumnDescription = "标识ID", ColumnDataType = "varchar", Length = 100)]
        public string IdentityId { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        [SugarColumn(ColumnDescription = "添加任务的节点类型", ColumnDataType = "varchar", Length = 50)]
        public string SystemType { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [SugarColumn(ColumnDescription = "说明", ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]
        public string Show { get; set; }

        /// <summary>
        /// 重试状态
        /// </summary>
        [SugarColumn(ColumnDescription = "重试状态")]
        public byte Status { get; set; }


        /// <summary>
        /// 发送参数
        /// </summary>
        [SugarColumn(ColumnDescription = "发送参数", ColumnDataType = "nvarchar", Length = 1500)]
        public string SendBody { get; set; }
        /// <summary>
        /// 重试配置
        /// </summary>
        [SugarColumn(ColumnDescription = "重试配置", ColumnDataType = "varchar", Length = 500)]
        public string RetryConfig { get; set; }


        /// <summary>
        /// 负责重试节点ID
        /// </summary>
        [SugarColumn(ColumnDescription = "负责重试节点ID")]
        public long RegServiceId { get; set; }
        /// <summary>
        /// 锁定状态
        /// </summary>
        [SugarColumn(ColumnDescription = "锁定状态")]
        public bool IsLock { get; set; }
        /// <summary>
        /// 下次重试时间
        /// </summary>
        [SugarColumn(ColumnDescription = "下次重试时间")]
        public long NextRetryTime { get; set; }

        /// <summary>
        /// 已经重试次数
        /// </summary>
        [SugarColumn(ColumnDescription = "已经重试次数")]
        public int RetryNum { get; set; }

        /// <summary>
        /// 最后次错误码
        /// </summary>
        [SugarColumn(ColumnDescription = "最后次错误码", ColumnDataType = "varchar", Length = 100)]
        public string ErrorCode { get; set; }


        /// <summary>
        /// 完成时间
        /// </summary>
        [SugarColumn(ColumnDescription = "完成时间")]
        public DateTime? ComplateTime { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime { get; set; }
    }
}
