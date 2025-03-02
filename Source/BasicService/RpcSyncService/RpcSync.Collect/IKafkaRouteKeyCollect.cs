using WeDonekRpc.Model.Kafka.Model;

namespace RpcSync.Collect
{
    public interface IKafkaRouteKeyCollect
    {
        void Add(int exchangeId, ExchangeRouteKey[] keys);
        ExchangeRouteKey[] GetRouteKey(int exchangeId);
    }
}