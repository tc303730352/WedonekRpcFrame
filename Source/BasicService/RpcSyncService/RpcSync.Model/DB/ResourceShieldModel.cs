namespace RpcSync.Model.DB
{
    using SqlSugar;

    [SugarTable("ResourceShield")]
    public class ResourceShieldModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long ResourceId { get; set; }

        public string ShieKey { get; set; }

        public long RpcMerId { get; set; }

        public string SystemType { get; set; }

        public long ServerId { get; set; }

        public int VerNum { get; set; }

        public short SortNum { get; set; }

        public string ResourcePath { get; set; }

        public long BeOverdueTime { get; set; }
    }
}
