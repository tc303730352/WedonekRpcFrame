using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_RpcMerId_SysTypeId", "RpcMerId", OrderByType.Asc, "SystemTypeId", OrderByType.Asc, true)]
    [SugarTable("RpcMerConfig", TableDescription = "集群节点类别负载均衡配置表")]
    public class RpcMerConfigModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
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
        [SugarColumn(ColumnDescription = "节点类别ID")]
        public long SystemTypeId
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡隔离
        /// </summary>
        [SugarColumn(ColumnDescription = "节点负载均衡时是否按照机房隔离")]
        public bool IsRegionIsolate
        {
            get;
            set;
        }
        /// <summary>
        /// 隔离级别
        /// </summary>
        [SugarColumn(ColumnDescription = "节点负载均衡时隔离级别 true=完全隔离只能访问同机房的节点  false=同机房的节点优先")]
        public bool IsolateLevel
        {
            get;
            set;
        }
        /// <summary>
        /// 负载均衡的方式
        /// </summary>
        [SugarColumn(ColumnDescription = "节点负载均衡的方式")]
        public byte BalancedType
        {
            get;
            set;
        }
        /// <summary>
        /// 当前正式版本号
        /// </summary>
        [SugarColumn(ColumnDescription = "当前正式版本号")]
        public int CurrentVer
        {
            get;
            set;
        }
    }
}
