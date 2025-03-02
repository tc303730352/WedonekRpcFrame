using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarIndex("IX_ErrorId", "ErrorId", OrderByType.Asc, false)]
    [SugarIndex("IX_ErrorId", "ErrorId", OrderByType.Asc, "Lang", OrderByType.Asc, true)]
    [SugarTable("ErrorLangMsg", tableDescription: "错误码文本信息对照表")]
    public class ErrorLangMsgModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "错误ID")]
        public long ErrorId
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "语言", Length = 20, ColumnDataType = "varchar")]
        public string Lang
        {
            get;
            set;
        }
        [SugarColumn(ColumnDescription = "错误文本信息", Length = 100)]
        public string Msg
        {
            get;
            set;
        }
    }
}
