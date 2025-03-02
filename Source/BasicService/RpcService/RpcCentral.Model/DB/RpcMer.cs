using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("RpcMer")]
    public class RpcMer
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id
        {
            get;
            set;
        }
        public string SystemName
        {
            get;
            set;
        }
        public string AppId
        {
            get;
            set;
        }
        public string AppSecret
        {
            get;
            set;
        }
        [SugarColumn(IsJson = true, ColumnDataType = "varchar(max)")]
        public string[] AllowServerIp
        {
            get;
            set;
        }
        public DateTime AddTime
        {
            get;
            set;
        }
    }
}
