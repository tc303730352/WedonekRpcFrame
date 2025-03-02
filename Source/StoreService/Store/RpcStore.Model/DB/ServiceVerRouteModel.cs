using SqlSugar;

namespace RpcStore.Model.DB
{
    [SugarTable("ServiceVerRoute")]
    public class ServiceVerRouteModel
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
