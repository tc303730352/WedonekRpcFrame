using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class RpcMerDAL : IRpcMerDAL
    {
        private readonly IRepository<RpcMer> _Db;
        public RpcMerDAL (IRepository<RpcMer> db)
        {
            this._Db = db;
        }
        public RpcMer GetRpcMer (string appId)
        {
            return this._Db.Get(a => a.AppId == appId);
        }

        public string GetMerAppId (long id)
        {
            return this._Db.Get(a => a.Id == id, a => a.AppId);
        }
    }
}
