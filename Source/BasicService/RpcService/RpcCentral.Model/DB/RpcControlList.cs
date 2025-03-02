using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("RpcControlList")]
    public class RpcControlList
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
        public string ServerIp
        {
            get;
            set;
        }
        public int Port
        {
            get;
            set;
        }
        public int RegionId
        {
            get;
            set;
        }
        public string Show
        {
            get;
            set;
        }
    }
}
