using WeDonekRpc.Model.Kafka.Model;
using RpcSync.Service.Model;

namespace RpcSync.Service.Interface
{
    public interface IKafkaExchangeService
    {
        KafkaExchange Get(string exchange);

        ExchangeRouteKey[] GetRouteKeys(string exchange);
        void Sync(ExchangeDatum[] exchange);
    }
}