using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerCurConfigDAL : IServerCurConfigDAL
    {
        private readonly IRepository<ServerCurConfigModel> _BasicDAL;
        public ServerCurConfigDAL (IRepository<ServerCurConfigModel> dal)
        {
            this._BasicDAL = dal;
        }

        public void Delete (long serverId)
        {
            _ = this._BasicDAL.Delete(a => a.ServerId == serverId);
        }

        public ServerCurConfigModel GetConfig (long serverId)
        {
            return this._BasicDAL.Get(a => a.ServerId == serverId);
        }
    }
}
