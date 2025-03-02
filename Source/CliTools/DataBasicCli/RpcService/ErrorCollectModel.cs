using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_ErrorCode", "ErrorCode", OrderByType.Asc, true)]
    [SugarTable("ErrorCollect", TableDescription = "错误信息")]
    public class ErrorCollectModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "错误ID")]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "错误码", Length = 100, ColumnDataType = "varchar")]
        public string ErrorCode
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "是否完善了提示文本")]
        public bool IsPerfect
        {
            get;
            set;
        }
    }
}
