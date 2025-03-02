using RpcCentral.Model;
using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RpcMerConfigDAL : IRpcMerConfigDAL
    {
        private readonly IRepository<RpcMerConfig> _Db;
        public RpcMerConfigDAL (IRepository<RpcMerConfig> db)
        {
            this._Db = db;
        }
        public MerConfig GetConfig (long rpcMerId, long sysTypeId)
        {
            return this._Db.Get<MerConfig>(c => c.RpcMerId == rpcMerId && c.SystemTypeId == sysTypeId);
        }
    }
}
