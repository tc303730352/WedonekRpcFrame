using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Model.Kafka.Model;
using RpcSync.DAL;

namespace RpcSync.Collect.Collect
{
    internal class KafkaRouteKeyCollect : IKafkaRouteKeyCollect
    {
        private IKafkaRouteKeyDAL _DAL;
        private ICacheController _Cache;
        public KafkaRouteKeyCollect(IKafkaRouteKeyDAL dal, ICacheController cache)
        {
            this._Cache = cache;
            this._DAL = dal;
        }
        public void Add(int exchangeId, ExchangeRouteKey[] keys)
        {
            this._DAL.Add(exchangeId, keys);
            this._Refresh(exchangeId);
        }
        private void _Refresh(int exchangeId)
        {
            string key = string.Concat("ExchangeKey_", exchangeId);
            this._Cache.Remove(key);
        }
        public ExchangeRouteKey[] GetRouteKey(int exchangeId)
        {
            string key = string.Concat("ExchangeKey_", exchangeId);
            if (this._Cache.TryGet(key, out ExchangeRouteKey[] keys))
            {
                return keys;
            }
            keys = this._DAL.GetRouteKey(exchangeId);
            if (keys == null)
            {
                keys = new ExchangeRouteKey[0];
            }
            else
            {
                keys = keys.OrderBy(a => a.RouteKey).ToArray();
            }
            this._Cache.Set(key, keys, new TimeSpan(10, 0, 0, 0));
            return keys;
        }

    }
}
