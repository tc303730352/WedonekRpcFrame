using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("ServerDictateLimit", TableDescription = "服务指令限流配置")]
    [SugarIndex("IX_ServerId", "ServerId", OrderByType.Asc, "Dictate", OrderByType.Asc, true)]
    public class ServerDictateLimitModel
    {
        /// <summary>
        /// 指令Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识Id")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        [SugarColumn(ColumnDescription = "服务节点id")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 限流指令
        /// </summary>
        [SugarColumn(ColumnDescription = "限流指令", Length = 50, ColumnDataType = "varchar")]
        public string Dictate
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
        /// 窗口大小（秒）
        /// </summary>
        [SugarColumn(ColumnDescription = "窗口大小（秒）")]
        public short LimitTime
        {
            get;
            set;
        }


        /// <summary>
        /// 桶大小
        /// </summary>
        public short BucketSize
        {
            get;
            set;
        }
        /// <summary>
        /// 桶溢出速度
        /// </summary>
        public short BucketOutNum
        {
            get;
            set;
        }
        /// <summary>
        /// 令牌最大数
        /// </summary>
        public short TokenNum
        {
            get;
            set;
        }
        /// <summary>
        /// 每秒添加令牌数
        /// </summary>
        public short TokenInNum
        {
            get;
            set;
        }
    }
}
