using SqlSugar;

namespace RpcCentral.Model.DB
{
    [SugarTable("RpcMerServerVer")]
    public class RpcMerServerVerModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long RpcMerId { get; set; }

        public long SystemTypeId { get; set; }

        public int CurrentVer { get; set; }
    }
}
