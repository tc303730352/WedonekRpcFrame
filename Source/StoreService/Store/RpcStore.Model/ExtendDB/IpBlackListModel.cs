using RpcStore.RemoteModel;
using SqlSugar;

namespace RpcStore.Model.ExtendDB
{
    [SugarTable("IpBlackList")]
    public class IpBlackListModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long RpcMerId { get; set; }

        public string SystemType { get; set; }

        public IpType IpType { get; set; }

        public string Ip6 { get; set; }
        public long Ip { get; set; }

        public long? EndIp { get; set; }

        public bool IsDrop { get; set; }

        public string Remark { get; set; }
    }
}
