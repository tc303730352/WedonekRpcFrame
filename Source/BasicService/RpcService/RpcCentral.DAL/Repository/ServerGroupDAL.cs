using RpcCentral.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcCentral.DAL.Repository
{
    internal class ServerGroupDAL : IServerGroupDAL
    {
        private readonly IRepository<ServerGroupModel> _Db;
        public ServerGroupDAL (IRepository<ServerGroupModel> db)
        {
            this._Db = db;
        }
        public string GetTypeVal (long groupId)
        {
            return this._Db.Get(a => a.Id == groupId, a => a.TypeVal);
        }
    }
}
