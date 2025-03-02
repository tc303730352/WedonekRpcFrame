using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_ContrainerId", "GroupId", OrderByType.Asc, "ContrainerId", OrderByType.Asc, true)]
    [SugarTable("ContainerList", TableDescription = "容器组中的容器信息")]
    public class ContainerListModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "容器ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 容器组
        /// </summary>
        [SugarColumn(ColumnDescription = "所在容器组ID")]
        public long GroupId { get; set; }
        /// <summary>
        /// 容器ID
        /// </summary>
        [SugarColumn(ColumnDescription = "容器的唯一标识ID", IsNullable = false, Length = 100, ColumnDataType = "varchar")]
        public string ContrainerId
        {
            get;
            set;
        }
        /// <summary>
        /// 虚拟MAC
        /// </summary>
        [SugarColumn(ColumnDescription = "容器的唯一标识ID", IsNullable = false, Length = 17, ColumnDataType = "varchar")]
        public string VirtualMac
        {
            get;
            set;
        }
        /// <summary>
        /// 容器内部IP
        /// </summary>
        [SugarColumn(ColumnDescription = "容器内部IP", IsNullable = false, Length = 36, ColumnDataType = "varchar")]
        public string InternalIp
        {
            get;
            set;
        }
        /// <summary>
        /// 容器内部端口号
        /// </summary>
        [SugarColumn(ColumnDescription = "容器内部端口号", IsNullable = false)]
        public int InternalPort
        {
            get;
            set;
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDataType = "smalldatetime", ColumnDescription = "创建时间")]
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
