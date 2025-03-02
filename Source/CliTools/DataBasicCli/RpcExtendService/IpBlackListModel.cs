using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("IpBlackList", "Ip黑名单表")]
    public class IpBlackListModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "标识Id")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId { get; set; }

        [SugarColumn(ColumnDescription = "服务类别值", ColumnDataType = "varchar", Length = 50)]
        public string SystemType { get; set; }

        [SugarColumn(ColumnDescription = "Ip类型")]
        public byte IpType { get; set; }
        [SugarColumn(ColumnDescription = "Ip6", ColumnDataType = "varchar", Length = 30)]
        public string Ip6 { get; set; }

        [SugarColumn(ColumnDescription = "起始的Ip4")]
        public long Ip { get; set; }
        [SugarColumn(ColumnDescription = "结束的Ip4")]
        public long? EndIp { get; set; }
        [SugarColumn(ColumnDescription = "是否删除")]
        public bool IsDrop { get; set; }
        [SugarColumn(ColumnDescription = "备注", Length = 50)]
        public string Remark { get; set; }
    }
}
