using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("DictateNodeRelation", "广播节点路由关系配置表")]
    public class DictateNodeRelationModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "子级ID")]
        public long SubId { get; set; }

        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "父级ID")]
        public long ParentId { get; set; }
    }
}
