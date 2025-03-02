using RpcCentral.Model.DB;
using WeDonekRpc.Model.Model;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServerClientLimitDAL : IServerClientLimitDAL
    {
        private readonly IRepository<ServerClientLimitModel> _Db;
        public ServerClientLimitDAL (IRepository<ServerClientLimitModel> db)
        {
            this._Db = db;
        }


        public ServerClientLimit GetClientLimit (long rpcMerId, long serverId)
        {
            return this._Db.Get<ServerClientLimit>(c => c.RpcMerId == rpcMerId && c.ServerId == serverId);
        }
    }
}
