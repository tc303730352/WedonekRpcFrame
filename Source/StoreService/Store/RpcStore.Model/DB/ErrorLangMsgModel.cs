using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("ErrorLangMsg")]
    public class ErrorLangMsgModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public long ErrorId
        {
            get;
            set;
        }
        public string Lang
        {
            get;
            set;
        }
        public string Msg
        {
            get;
            set;
        }
    }
}
