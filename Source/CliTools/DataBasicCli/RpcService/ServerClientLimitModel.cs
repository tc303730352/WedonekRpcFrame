using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点客户端限流配置
    /// </summary>
    [SugarTable("ServerClientLimit", TableDescription = "节点客户端限流配置")]
    [SugarIndex("IX_ServerId", "RpcMerId", OrderByType.Asc, "ServerId", OrderByType.Asc, true)]
    public class ServerClientLimitModel
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群Id
        /// </summary>
        [SugarColumn(ColumnDescription = "应用集群ID")]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 节点Id
        /// </summary>
        [SugarColumn(ColumnDescription = "应用节点ID")]
        public long ServerId
        {
            get;
            set;
        }

        /// <summary>
        /// 限制类型
        /// </summary>
        [SugarColumn(ColumnDescription = "限流类型")]
        public byte LimitType
        {
            get;
            set;
        }
        /// <summary>
        /// 限制发送数量
        /// </summary>
        [SugarColumn(ColumnDescription = "限制发送数量")]
        public int LimitNum
        {
            get;
            set;
        }
        /// <summary>
        /// 限定窗口时间
        /// </summary>
        [SugarColumn(ColumnDescription = "限定窗口时间")]
        public short LimitTime
        {
            get;
            set;
        }
        /// <summary>
        /// 限定令牌数
        /// </summary>
        [SugarColumn(ColumnDescription = "限定令牌数")]
        public short TokenNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每秒写入令牌数量
        /// </summary>
        [SugarColumn(ColumnDescription = "每秒写入令牌数量")]
        public short TokenInNum
        {
            get;
            set;
        }
    }
}
