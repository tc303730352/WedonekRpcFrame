using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("ServerTransmitScheme", TableDescription = "负载均衡方案表")]
    [SugarIndex("IX_SystemTypeId_ServerId", "SystemTypeId", OrderByType.Asc, "RpcMerId", OrderByType.Asc, false)]
    public class ServerTransmitSchemeModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "负载均衡方案ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "服务类别Id")]
        public long SystemTypeId
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
        /// <summary>
        /// 方案名称
        /// </summary>
        [SugarColumn(ColumnDescription = "方案名称", Length = 50, IsNullable = false)]
        public string Scheme
        {
            get;
            set;
        }

        /// <summary>
        /// 负载均衡类型
        /// </summary>
        [SugarColumn(ColumnDescription = "负载均衡类型")]
        public byte TransmitType
        {
            get;
            set;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        [SugarColumn(ColumnDescription = "应用的版本号")]
        public int VerNum
        {
            get;
            set;
        }
        /// <summary>
        /// 备注说明
        /// </summary>
        [SugarColumn(ColumnDescription = "备注说明", Length = 50)]
        public string Show
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
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime { get; set; }
    }
}
