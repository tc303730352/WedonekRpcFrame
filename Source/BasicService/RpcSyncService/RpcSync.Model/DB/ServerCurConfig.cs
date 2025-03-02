namespace RpcSync.Model.DB
{
    [SqlSugar.SugarTable("ServerCurConfig")]
    public class ServerCurConfigModel
    {
        [SqlSugar.SugarColumn(IsPrimaryKey = true)]
        public long ServerId { get; set; }

        public string CurConfig { get; set; }

        public DateTime UpTime { get; set; }
    }
}
