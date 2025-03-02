using RpcStore.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerClientLimitDAL : IServerClientLimitDAL
    {
        private readonly IRepository<ServerClientLimitModel> _BasicDAL;
        public ServerClientLimitDAL (IRepository<ServerClientLimitModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long Add (ServerClientLimitModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public void Set (long id, ServerClientLimit config)
        {
            if (!this._BasicDAL.Update(config, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.client.limit.set.error");
            }
        }
        public ServerClientLimitModel Get (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(c => c.Id == id))
            {
                throw new ErrorException("rpc.store.client.limit.delete.error");
            }
        }
        public ServerClientLimitModel Get (long rpcMerId, long serverId)
        {
            return this._BasicDAL.Get(c => c.RpcMerId == rpcMerId && c.ServerId == serverId);
        }
        public ServerClientLimitModel[] Gets (long[] rpcMerId, long serverId)
        {
            return this._BasicDAL.Gets(c => rpcMerId.Contains(c.RpcMerId) && c.ServerId == serverId);
        }
        public void Clear (long rpcMerId, long serverId)
        {
            _ = this._BasicDAL.Delete(a => a.RpcMerId == rpcMerId && a.ServerId == serverId);
        }
        public void Clear (long serverId)
        {
            _ = this._BasicDAL.Delete(c => c.ServerId == serverId);
        }
    }
}
