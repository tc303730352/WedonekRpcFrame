using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("DictItem", tableDescription: "字典明细表")]
    public class DictItemModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "字典编号", Length = 10, ColumnDataType = "varchar")]
        public string DictCode
        {
            get;
            set;
        }
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "字典条目值", Length = 50, ColumnDataType = "varchar")]
        public string ItemCode
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "父级ItemCode", Length = 50, ColumnDataType = "varchar")]
        public string PrtItemCode
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "条目文本", Length = 50)]
        public string ItemText
        {
            get;
            set;
        }

    }
}
