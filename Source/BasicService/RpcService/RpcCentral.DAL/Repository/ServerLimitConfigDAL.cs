using RpcCentral.Model.DB;
using WeDonekRpc.Model.Model;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServerLimitConfigDAL : IServerLimitConfigDAL
    {
        private readonly IRepository<ServerLimitConfigModel> _Db;
        public ServerLimitConfigDAL (IRepository<ServerLimitConfigModel> db)
        {
            this._Db = db;
        }
        public ServerLimitConfig GetLimitConfig (long serverId)
        {
            return this._Db.Get<ServerLimitConfig>(a => a.ServerId == serverId);
        }

    }
}
