using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ReduceInRankConfigDAL : IReduceInRankConfigDAL
    {
        private readonly IRepository<ReduceInRankConfig> _Db;
        public ReduceInRankConfigDAL (IRepository<ReduceInRankConfig> db)
        {
            this._Db = db;
        }

        public ReduceInRankConfig GetReduceInRank (long rpcMerId, long servrId)
        {
            return this._Db.Get(a => a.RpcMerId == rpcMerId && a.ServerId == servrId);
        }

    }
}
