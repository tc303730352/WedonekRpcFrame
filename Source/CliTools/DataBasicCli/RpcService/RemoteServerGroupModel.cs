using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 远程服务组别
    /// </summary>
    [SugarIndex("IX_RpcMerId", "RpcMerId", OrderByType.Asc, false)]
    [SugarIndex("IX_SystemType", "SystemType", OrderByType.Asc, false)]
    [SugarIndex("IX_RemoteServerGroup", "RpcMerId", OrderByType.Asc, "SystemType", OrderByType.Asc, false)]
    [SugarIndex("IX_ServerId", "RpcMerId", OrderByType.Asc, "ServerId", OrderByType.Asc, true)]
    [SugarTable("RemoteServerGroup", TableDescription = "远程服务组别")]
    public class RemoteServerGroupModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 集群ID
        /// </summary>
        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 服务Id
        /// </summary>
        [SugarColumn(ColumnDescription = "服务节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "节点所属机房")]
        public int RegionId
        {
            get;
            set;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        [SugarColumn(ColumnDescription = "节点类别")]
        public long SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 类型值
        /// </summary>
        [SugarColumn(ColumnDescription = "节点类别值", Length = 50, ColumnDataType = "varchar")]
        public string TypeVal
        {
            get;
            set;
        }
        /// <summary>
        /// 服务类型
        /// </summary>
        [SugarColumn(ColumnDescription = "节点类型")]
        public byte ServiceType { get; set; }
        /// <summary>
        /// 权重
        /// </summary>
        [SugarColumn(ColumnDescription = "负载权重")]
        public int Weight
        {
            get;
            set;
        }
        /// <summary>
        /// 是否持有
        /// </summary>
        [SugarColumn(ColumnDescription = "集群拥有该节点")]
        public bool IsHold
        {
            get;
            set;
        }
    }
}
