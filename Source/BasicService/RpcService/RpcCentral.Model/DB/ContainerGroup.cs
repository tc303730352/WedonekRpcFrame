using WeDonekRpc.Model;

namespace RpcCentral.Model.DB
{
    [SqlSugar.SugarTable("ContainerGroup")]
    public class ContainerGroup
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }

        public int RegionId { get; set; }

        public string HostMac { get; set; }

        public ContainerType ContainerType { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public string HostIp { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
