using WeDonekRpc.Helper.Config;

namespace RpcStore.Model.DB
{
    [SqlSugar.SugarTable("ServerCurConfig")]
    public class ServerCurConfigModel
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long ServerId { get; set; }

        [SqlSugar.SugarColumn(IsJson = true)]
        public Dictionary<string, ConfigItemModel> CurConfig { get; set; }

        public DateTime UpTime { get; set; }
    }
}
