using SqlSugar;

namespace DataBasicCli.RpcService
{
    /// <summary>
    /// 服务节点版本配置
    /// </summary>
    [SugarIndex("IX_SchemeId", "SchemeId", OrderByType.Asc, false)]
    [SugarIndex("IX_SchemeId_SystemTypeId", "SchemeId", OrderByType.Asc, "SystemTypeId", OrderByType.Asc, true)]
    [SugarTable("ServiceVerConfig", "服务节点版本配置")]
    public class ServiceVerConfigModel
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "配置方案ID")]
        public long SchemeId
        {
            get;
            set;
        }
        /// <summary>
        /// 版本号 版本格式 "001.01.01" 去掉 点
        /// </summary>
        [SugarColumn(ColumnDescription = "版本号 版本格式0010101")]
        public int VerNum { get; set; }
        /// <summary>
        /// 系统节点类型
        /// </summary>
        [SugarColumn(ColumnDescription = "系统节点类型")]
        public long SystemTypeId { get; set; }

    }
}
