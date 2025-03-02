using RpcSync.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class KafkaExchangeDAL : IKafkaExchangeDAL
    {
        private readonly IRepository<KafkaExchangeModel> _BasicDAL;
        public KafkaExchangeDAL (IRepository<KafkaExchangeModel> dal)
        {
            this._BasicDAL = dal;
        }

        public int SyncExchange (string exchange)
        {
            int id = this._BasicDAL.Get(c => c.Exchange == exchange, c => c.Id);
            if (id != 0)
            {
                return id;
            }
            return (int)this._BasicDAL.InsertReutrnIdentity(new KafkaExchangeModel
            {
                Exchange = exchange
            });
        }
    }
}
