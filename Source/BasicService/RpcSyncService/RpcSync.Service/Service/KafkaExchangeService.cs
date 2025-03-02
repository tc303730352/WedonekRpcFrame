using WeDonekRpc.Helper;

using WeDonekRpc.Model.Kafka.Model;
using RpcSync.Collect;
using RpcSync.Service.Interface;
using RpcSync.Service.Model;

namespace RpcSync.Service.Service
{
    internal class KafkaExchangeService : IKafkaExchangeService
    {
        private IKafkaExchangeCollect _Exchange;
        private IKafkaRouteKeyCollect _ExchangeKey;
        public KafkaExchangeService(IKafkaExchangeCollect exchange,
            IKafkaRouteKeyCollect exchangeKey)
        {
            this._Exchange = exchange;
            this._ExchangeKey = exchangeKey;
        }

        public KafkaExchange Get(string exchange)
        {
            int id = this._Exchange.SyncExchange(exchange);
            ExchangeRouteKey[] keys = this._ExchangeKey.GetRouteKey(id);
            return new KafkaExchange
            {
                ExchangeId = id,
                RouteKey = keys
            };
        }
        public ExchangeRouteKey[] GetRouteKeys(string exchange)
        {
            int id = this._Exchange.SyncExchange(exchange);
           return this._ExchangeKey.GetRouteKey(id);
        }
        public void Sync(ExchangeDatum[] exchange)
        {
            exchange.ForEach(a =>
             {
                 KafkaExchange obj = this.Get(a.Exchange);
                 if (obj.RouteKey.Length != 0)
                 {
                     a.RouteKey = a.RouteKey.Remove(a => obj.RouteKey.IsExists(a));
                 }
                 if (a.RouteKey.Length == 0)
                 {
                     return;
                 }
                this._ExchangeKey.Add(exchangeId: obj.ExchangeId, a.RouteKey);
             });
        }
    }
}
