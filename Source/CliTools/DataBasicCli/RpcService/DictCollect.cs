using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("DictCollect", tableDescription: "字典表")]
    public class DictCollect
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "字典编号", Length = 10, ColumnDataType = "varchar")]
        public string DictCode
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "字典名", Length = 20)]
        public string DictName
        {
            get;
            set;
        }
    }
}
