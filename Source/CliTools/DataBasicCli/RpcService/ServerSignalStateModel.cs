using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点链接状态
    /// </summary>
    [SugarTable("ServerSignalState", TableDescription = "服务节点链接状态")]
    public class ServerSignalStateModel
    {
        /// <summary>
        /// 服务节点ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "发起服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 链接的服务节点
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "目的地服务节点ID")]
        public long RemoteId
        {
            get;
            set;
        }
        /// <summary>
        /// 链接数
        /// </summary>
        [SugarColumn(ColumnDescription = "链接数")]
        public int ConNum
        {
            get;
            set;
        }
        /// <summary>
        /// 平均响应时间
        /// </summary>
        [SugarColumn(ColumnDescription = "平均响应时间")]
        public int AvgTime
        {
            get;
            set;
        }
        /// <summary>
        /// 发送量
        /// </summary>
        [SugarColumn(ColumnDescription = "发送量")]
        public int SendNum
        {
            get;
            set;
        }
        /// <summary>
        /// 错误量
        /// </summary>
        [SugarColumn(ColumnDescription = "错误量")]
        public int ErrorNum
        {
            get;
            set;
        }
        /// <summary>
        /// 可用状态
        /// </summary>
        [SugarColumn(ColumnDescription = "可用状态")]
        public byte UsableState
        {
            get;
            set;
        }
        /// <summary>
        /// 同步时间
        /// </summary>
        [SugarColumn(ColumnDescription = "同步时间")]
        public DateTime SyncTime
        {
            get;
            set;
        }
    }
}
