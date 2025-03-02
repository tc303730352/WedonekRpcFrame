using SqlSugar;

namespace RpcStore.Model.ExtendDB
{
    [SugarTable("DictateNodeRelation")]
    public class DictateNodeRelationModel
    {
        [SugarColumn(IsPrimaryKey =true)]
        public long SubId { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public long ParentId { get; set; }
    }
}
