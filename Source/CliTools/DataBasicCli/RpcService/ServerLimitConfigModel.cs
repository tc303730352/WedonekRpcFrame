using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点限流配置
    /// </summary>
    [SugarTable("ServerLimitConfig", TableDescription = "服务节点限流配置")]
    public class ServerLimitConfigModel
    {
        /// <summary>
        /// 节点Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsEnable
        {
            get;
            set;
        }
        /// <summary>
        /// 限流类型
        /// </summary>
        [SugarColumn(ColumnDescription = "限流类型")]
        public byte LimitType
        {
            get;
            set;
        }
        /// <summary>
        /// 最大流量
        /// </summary>
        [SugarColumn(ColumnDescription = "最大流量")]
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 窗口大小
        /// </summary>
        [SugarColumn(ColumnDescription = "窗口大小")]
        public short LimitTime
        {
            get;
            set;
        }

        /// <summary>
        /// 操过限制的时候是否启用桶
        /// </summary>
        [SugarColumn(ColumnDescription = "操过限制的时候是否启用桶")]
        public bool IsEnableBucket
        {
            get;
            set;
        }
        /// <summary>
        /// 桶大小
        /// </summary>
        [SugarColumn(ColumnDescription = "桶大小")]
        public short BucketSize
        {
            get;
            set;
        }
        /// <summary>
        /// 桶溢出速度
        /// </summary>
        [SugarColumn(ColumnDescription = "桶溢出速度")]
        public short BucketOutNum
        {
            get;
            set;
        }
        /// <summary>
        /// 令牌总量
        /// </summary>
        [SugarColumn(ColumnDescription = "令牌总量")]
        public short TokenNum { get; set; }
        /// <summary>
        /// 令牌每秒新增数
        /// </summary>
        [SugarColumn(ColumnDescription = "令牌每秒新增数")]
        public short TokenInNum { get; set; }
    }
}
