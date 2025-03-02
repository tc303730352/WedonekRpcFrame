using WeDonekRpc.Model;
using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerRunStateDAL : IServerRunStateDAL
    {
        private readonly IRepository<ServerRunStateModel> _BasicDAL;

        public ServerRunStateDAL (IRepository<ServerRunStateModel> dal)
        {
            this._BasicDAL = dal;
        }

        public ServerRunStateModel Get (long serverId)
        {
            return this._BasicDAL.Get(c => c.ServerId == serverId);
        }
        public void Delete (long serverId)
        {
            _ = this._BasicDAL.Delete(c => c.ServerId == serverId);
        }
        public ServerRunStateModel[] Query (IBasicPage paging, out int count)
        {
            paging.InitOrderBy("ServerId", true);
            return this._BasicDAL.Query(paging, out count);
        }

    }
}
