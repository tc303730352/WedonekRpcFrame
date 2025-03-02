using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点降级配置
    /// </summary>
    [SugarIndex("IX_RpcMerId", "RpcMerId", OrderByType.Asc, false)]
    [SugarIndex("IX_ServerId", "ServerId", OrderByType.Asc, "RpcMerId", OrderByType.Asc, true)]
    [SugarTable("ReduceInRankConfig", TableDescription = "服务节点降级配置表")]
    public class ReduceInRankConfigModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "数据ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(ColumnDescription = "应用集群ID")]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        [SugarColumn(ColumnDescription = "应用服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsEnable { get; set; }
        /// <summary>
        /// 限制数
        /// </summary>
        [SugarColumn(ColumnDescription = "触发限制错误量")]
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 熔断错误数
        /// </summary>
        [SugarColumn(ColumnDescription = "触发熔断的错误量")]
        public int FusingErrorNum
        {
            get;
            set;
        }

        /// <summary>
        /// 刷新统计数的时间(秒)
        /// </summary>
        [SugarColumn(ColumnDescription = "刷新错误统计数的时间(秒)")]
        public int RefreshTime { get; set; }
        /// <summary>
        /// 最短融断时长
        /// </summary>
        [SugarColumn(ColumnDescription = "最短融断时长(秒)")]
        public int BeginDuration
        {
            get;
            set;
        }
        /// <summary>
        /// 最长熔断时长
        /// </summary>
        [SugarColumn(ColumnDescription = "最短融断时长(秒)")]
        public int EndDuration
        {
            get;
            set;
        }

    }
}
