using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("BroadcastErrorLog", TableDescription = "广播错误日志表")]
    public class BroadcastErrorLogModel
    {
        /// <summary>
        /// 日志Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "广播日志ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        [SugarColumn(ColumnDescription = "错误信息")]
        public string Error
        {
            get;
            set;
        }
        /// <summary>
        /// 消息Key
        /// </summary>
        [SugarColumn(ColumnDescription = "消息指令", ColumnDataType = "varchar", Length = 50)]
        public string MsgKey
        {
            get;
            set;
        }
        /// <summary>
        /// 来源服务节点ID
        /// </summary>
        [SugarColumn(ColumnDescription = "来源节点ID")]
        public long SourceId { get; set; }
        /// <summary>
        /// 消息体
        /// </summary>
        [SugarColumn(ColumnDescription = "消息体", ColumnDataType = "varchar")]
        public string MsgBody { get; set; }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [SugarColumn(ColumnDescription = "目的地节点Id")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 系统类型
        /// </summary>
        [SugarColumn(ColumnDescription = "目的地服务类别")]
        public string SysTypeVal
        {
            get;
            set;
        }

        /// <summary>
        /// 消息源
        /// </summary>
        [SugarColumn(ColumnDescription = "消息源", ColumnDataType = "text")]
        public string MsgSource { get; set; }

        /// <summary>
        /// 广播方式
        /// </summary>
        [SugarColumn(ColumnDescription = "广播方式")]
        public byte BroadcastType { get; set; }
        /// <summary>
        /// 路由健
        /// </summary>
        [SugarColumn(ColumnDescription = "路由健", ColumnDataType = "varchar", Length = 50)]
        public string RouteKey { get; set; }
        /// <summary>
        /// 集群Id
        /// </summary>
        [SugarColumn(ColumnDescription = "集群Id")]
        public long RpcMerId
        {
            get;
            set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
