using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerEnvironmentDAL : IServerEnvironmentDAL
    {
        private readonly IRepository<ServerEnvironmentModel> _Db;
        public ServerEnvironmentDAL (IRepository<ServerEnvironmentModel> db)
        {
            this._Db = db;
        }

        public void Clear (long serverId)
        {
            _ = this._Db.Delete(a => a.ServerId == serverId);
        }

        public ServerEnvironmentModel Get (long serverId)
        {
            return this._Db.Get(a => a.ServerId == serverId);
        }

    }
}
