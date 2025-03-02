using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServerSignalStateDAL : IServerSignalStateDAL
    {
        private readonly IRepository<ServerSignalState> _Db;
        public ServerSignalStateDAL (IRepository<ServerSignalState> db)
        {
            this._Db = db;
        }

        public bool SyncState (ServerSignalState[] states)
        {
            return this._Db.AddOrUpdate(new List<ServerSignalState>(states));
        }
    }
}
