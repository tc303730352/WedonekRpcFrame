using WeDonekRpc.CacheClient.Interface;
using RpcSync.DAL;

namespace RpcSync.Collect.Collect
{
    internal class KafkaExchangeCollect : IKafkaExchangeCollect
    {
        private IKafkaExchangeDAL _BasicDAL;
        private ICacheController _Cache;
        public KafkaExchangeCollect(IKafkaExchangeDAL dal, ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = dal;
        }

        public int SyncExchange(string exchange)
        {
            string key = string.Concat("Exchange_", exchange);
            if(this._Cache.TryGet(key,out int id))
            {
                return id;
            }
            id= this._BasicDAL.SyncExchange(exchange);
            this._Cache.Set(key, id, new TimeSpan(30, 0, 0, 0));
            return id;
        }
    }
}
