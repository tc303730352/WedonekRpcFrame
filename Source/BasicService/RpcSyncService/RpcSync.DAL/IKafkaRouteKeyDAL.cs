using WeDonekRpc.Model.Kafka.Model;

namespace RpcSync.DAL
{
    public interface IKafkaRouteKeyDAL
    {
        void Add(int exchangeId, ExchangeRouteKey[] keys);
        ExchangeRouteKey[] GetRouteKey(int exchangeId);
    }
}