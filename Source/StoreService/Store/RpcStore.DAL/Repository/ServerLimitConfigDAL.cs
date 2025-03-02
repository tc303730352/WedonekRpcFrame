using WeDonekRpc.Helper;
using WeDonekRpc.Model.Model;
using RpcStore.Model.DB;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerLimitConfigDAL : IServerLimitConfigDAL
    {
        private readonly IRepository<ServerLimitConfigModel> _BasicDAL;

        public ServerLimitConfigDAL (IRepository<ServerLimitConfigModel> dal)
        {
            this._BasicDAL = dal;
        }

        public void Add (ServerLimitConfigModel add)
        {
            this._BasicDAL.Insert(add);
        }
        public void Set (long serverId, ServerLimitConfig config)
        {
            if (!this._BasicDAL.Update(config, a => a.ServerId == serverId))
            {
                throw new ErrorException("rpc.store.server.limit.config.set.error");
            }
        }
        public void Delete (long serverId)
        {
            if (!this._BasicDAL.Delete(a => a.ServerId == serverId))
            {
                throw new ErrorException("rpc.store.server.limit.config.delete.error");
            }
        }
        public ServerLimitConfigModel Get (long serverId)
        {
            return this._BasicDAL.Get(c => c.ServerId == serverId);
        }
    }
}
