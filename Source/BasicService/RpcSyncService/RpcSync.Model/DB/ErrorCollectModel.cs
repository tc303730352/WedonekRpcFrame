using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("ErrorCollect")]
    public class ErrorCollectModel
    {
        [SugarColumn(IsPrimaryKey =true)]
        public long Id
        {
            get;
            set;
        }
        public string ErrorCode
        {
            get;
            set;
        }
        public bool IsPerfect
        {
            get;
            set;
        }
    }
}
