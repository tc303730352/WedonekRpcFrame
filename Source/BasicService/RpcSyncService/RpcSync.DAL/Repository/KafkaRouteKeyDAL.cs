using WeDonekRpc.Client;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model.Kafka.Model;
using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class KafkaRouteKeyDAL : IKafkaRouteKeyDAL
    {
        private readonly IRepository<KafkaRouteKeyModel> _BasicDAL;
        public KafkaRouteKeyDAL (IRepository<KafkaRouteKeyModel> dal)
        {
            this._BasicDAL = dal;
        }
        public ExchangeRouteKey[] GetRouteKey (int exchangeId)
        {
            return this._BasicDAL.Gets(c => c.ExchangeId == exchangeId, c => new ExchangeRouteKey
            {
                Queue = c.Queue,
                RouteKey = c.RouteKey
            });
        }
        public void Add (int exchangeId, ExchangeRouteKey[] keys)
        {
            this._BasicDAL.Insert(keys.ConvertMap<ExchangeRouteKey, KafkaRouteKeyModel>((a, b) =>
            {
                b.Id = IdentityHelper.CreateId();
                b.ExchangeId = exchangeId;
                return b;
            }));
        }
    }
}
