using SqlSugar;

namespace DataBasicCli.RpcService
{
    [SugarTable("ServerCurConfig", TableDescription = "服务节点当前配置表")]
    public class ServerCurConfigModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "服务节点ID")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "节点当前配置", ColumnDataType = "text")]
        public string CurConfig { get; set; }

        [SugarColumn(ColumnDescription = "上传时间")]
        public DateTime UpTime { get; set; }
    }
}
