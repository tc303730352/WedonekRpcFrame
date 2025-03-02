
using WeDonekRpc.Model.Kafka.Model;

namespace RpcSync.Service.Model
{
    public class KafkaExchange
    {
        public int ExchangeId
        {
            get;
            set;
        }

        public ExchangeRouteKey[] RouteKey
        {
            get;
            set;
        }
    }
}
