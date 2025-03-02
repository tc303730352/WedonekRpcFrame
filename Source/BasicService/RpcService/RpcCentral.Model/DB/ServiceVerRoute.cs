using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("ServiceVerRoute")]
    public class ServiceVerRoute
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long SchemeId { get; set; }
        public long VerId { get; set; }
        public long SystemTypeId { get; set; }

        public int ToVerId
        {
            get;
            set;
        }
    }
}
