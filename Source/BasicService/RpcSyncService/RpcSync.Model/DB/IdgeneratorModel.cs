namespace RpcSync.Model.DB
{
    using SqlSugar;

    [SugarTable("Idgenerator")]
    public class IdgeneratorModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int WorkId { get; set; }

        public long SystemTypeId { get; set; }

        public string Mac { get; set; }

        public int ServerIndex { get; set; }
    }
}
