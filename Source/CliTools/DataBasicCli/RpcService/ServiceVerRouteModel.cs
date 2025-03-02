using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_SchemeId", "SchemeId", OrderByType.Asc, false)]
    [SugarTable("ServiceVerRoute", TableDescription = "服务节点版本路由配置")]
    [SugarIndex("IX_VerId", "VerId", OrderByType.Asc, false)]
    public class ServiceVerRouteModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "方案ID")]
        public long SchemeId { get; set; }

        [SugarColumn(ColumnDescription = "版本ID")]
        public long VerId { get; set; }

        [SugarColumn(ColumnDescription = "系统节点类型ID")]
        public long SystemTypeId { get; set; }

        [SugarColumn(ColumnDescription = "目的地版本号")]
        public int ToVerId
        {
            get;
            set;
        }
    }
}
