namespace RpcSync.Model
{
    public class AccreditTokenSet
    {
        [SqlSugar.SugarColumn(IsJson =true,ColumnDataType ="varchar(1000)")]
        public string[] AccreditRole { get; set; }

        public string State { get; set; }
        public int StateVer { get; set; }

        public DateTime? Expire { get; set; }

        public DateTime OverTime { get; set; }
    }
}
