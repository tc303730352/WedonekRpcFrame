namespace RpcSync.Model.DB
{
    using SqlSugar;

    [SugarTable("DictateNode")]
    public class DictateNodeModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long ParentId { get; set; }
        public string Dictate { get; set; }

        public string DictateName { get; set; }

        public bool IsEndpoint { get; set; }
    }
}
