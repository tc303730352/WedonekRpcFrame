using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_SystemType", "SystemType", OrderByType.Asc, "IsEnable", OrderByType.Asc, false)]
    [SugarTable("SysConfig", "系统配置表")]
    public class SysConfigModel
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "配置ID")]
        public long Id { get; set; }
        /// <summary>
        /// 集群Id
        /// </summary>
        [SugarColumn(ColumnDescription = "集群Id")]
        public long RpcMerId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "服务类型")]
        public byte ServiceType
        {
            get;
            set;
        }
        /// <summary>
        /// 服务节点Id
        /// </summary>
        [SugarColumn(ColumnDescription = "应用节点ID")]
        public long ServerId
        {
            get;
            set;
        }
        /// <summary>
        /// 机房ID
        /// </summary>
        [SugarColumn(ColumnDescription = "应用机房ID")]
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组
        /// </summary>
        [SugarColumn(ColumnDescription = "应用容器组")]
        public long ContainerGroup
        {
            get;
            set;
        }
        /// <summary>
        /// 客户端版本号
        /// </summary>
        [SugarColumn(ColumnDescription = "应用版本号")]
        public int VerNum { get; set; }
        /// <summary>
        /// 系统类目
        /// </summary>
        [SugarColumn(ColumnDescription = "应用服务类别", Length = 50, ColumnDataType = "varchar")]
        public string SystemType
        {
            get;
            set;
        }
        /// <summary>
        /// 配置名
        /// </summary>
        [SugarColumn(ColumnDescription = "配置名", Length = 50, ColumnDataType = "varchar")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 值类型
        /// </summary>
        [SugarColumn(ColumnDescription = "配置值类型")]
        public byte ValueType
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        [SugarColumn(ColumnDescription = "配置值")]
        public string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 配置说明
        /// </summary>
        [SugarColumn(ColumnDescription = "配置说明", Length = 50, IsNullable = true)]
        public string Show
        {
            get;
            set;
        }
        /// <summary>
        /// 配置权限
        /// </summary>
        [SugarColumn(ColumnDescription = "配置权限")]
        public int Prower
        {
            get;
            set;
        }
        /// <summary>
        /// 配置类型
        /// </summary>
        [SugarColumn(ColumnDescription = "配置类型")]
        public byte ConfigType { get; set; }
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
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间")]
        public DateTime ToUpdateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 配置摸版
        /// </summary>
        [SugarColumn(ColumnDescription = "配置摸版", Length = 50, IsNullable = true)]
        public string TemplateKey
        {
            get;
            set;
        }
    }
}
