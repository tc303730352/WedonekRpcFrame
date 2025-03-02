using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("ServerPublicScheme", TableDescription = "服务节点发布方案")]
    public class ServerPublicSchemeModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        /// <summary>
        /// 所属集群
        /// </summary>
        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId
        {
            get;
            set;
        }
        /// <summary>
        /// 方案名
        /// </summary>
        [SugarColumn(ColumnDescription = "发布方案名", Length = 50, IsNullable = false)]
        public string SchemeName { get; set; }
        /// <summary>
        /// 方案说明
        /// </summary>
        [SugarColumn(ColumnDescription = "方案说明", Length = 100, IsNullable = false)]
        public string SchemeShow { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public byte Status { get; set; }
        [SugarColumn(ColumnDescription = "最后更新时间")]
        public DateTime LastTime { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime { get; set; }
    }
}
