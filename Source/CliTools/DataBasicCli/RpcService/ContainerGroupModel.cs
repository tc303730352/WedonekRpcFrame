using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_HostMac_Type", "HostMac", OrderByType.Asc, "ContainerType", OrderByType.Asc, true)]
    [SugarTable("ContainerGroup", TableDescription = "容器组")]
    public class ContainerGroupModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 区域ID
        /// </summary>
        [SugarColumn(ColumnDescription = "所在机房Id")]
        public int RegionId
        {
            get;
            set;
        }
        /// <summary>
        /// 容器编号
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 17, ColumnDescription = "宿主机MAC", ColumnDataType = "varchar")]
        public string HostMac
        {
            get;
            set;
        }
        /// <summary>
        /// 容器类型
        /// </summary>
        [SugarColumn(ColumnDescription = "容器类型")]
        public byte ContainerType
        {
            get;
            set;
        }
        /// <summary>
        /// 节点组名
        /// </summary>
        [SugarColumn(IsNullable = false, Length = 20, ColumnDescription = "容器组名称")]
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 150, ColumnDescription = "备注")]
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// 宿主内部IP
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 36, ColumnDescription = "宿主内部IP", ColumnDataType = "varchar")]
        public string HostIp
        {
            get;
            set;
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDataType = "smalldatetime", ColumnDescription = "创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
