using WeDonekRpc.Model.Kafka.Model;

namespace WeDonekRpc.Model.Kafka
{
    [IRemoteConfig("AddKafkaExchange", "sys.sync", false)]
    public class AddKafkaExchange
    {
        public ExchangeDatum[] Exchange
        {
            get;
            set;
        }

    }
}
