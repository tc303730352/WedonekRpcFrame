using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ClientLimit.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerClientLimitCollect : IServerClientLimitCollect
    {
        private readonly IServerClientLimitDAL _BasicDAL;

        public ServerClientLimitCollect (IServerClientLimitDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public bool Sync (ClientLimitDatum add)
        {
            ServerClientLimitModel limit = this._BasicDAL.Get(add.RpcMerId, add.ServerId);
            if (limit == null)
            {
                _ = this._BasicDAL.Add(add.ConvertMap<ClientLimitDatum, ServerClientLimitModel>());
                return true;
            }
            else if (!add.IsEquals(limit))
            {
                this._BasicDAL.Set(limit.Id, add.ConvertMap<ClientLimitDatum, ServerClientLimit>());
                return true;
            }
            return false;
        }


        public void Delete (ServerClientLimitModel config)
        {
            this._BasicDAL.Delete(config.Id);
        }
        public ServerClientLimitModel Get (long rpcMerId, long serverId)
        {
            return this._BasicDAL.Get(rpcMerId, serverId);
        }
        public ServerClientLimitModel Get (long id)
        {
            ServerClientLimitModel limit = this._BasicDAL.Get(id);
            if (limit == null)
            {
                throw new ErrorException("rpc.store.limit.not.find");
            }
            return limit;
        }

        public ServerClientLimitModel[] Gets (long[] rpcMerId, long serverId)
        {
            return this._BasicDAL.Gets(rpcMerId, serverId);
        }
    }
}
