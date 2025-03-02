using SqlSugar;

namespace RpcSync.Model.DB
{
    [SugarTable("KafkaExchange")]
    public class KafkaExchangeModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get;
            set;
        }
        public string Exchange
        {
            get;
            set;
        }
    }
}
