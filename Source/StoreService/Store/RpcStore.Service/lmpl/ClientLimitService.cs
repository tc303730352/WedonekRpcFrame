using RpcManageClient;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ClientLimit.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

namespace RpcStore.Service.lmpl
{
    internal class ClientLimitService : IClientLimitService
    {
        private readonly IServerClientLimitCollect _Limit;
        private readonly IRemoteGroupCollect _RemoteGroup;
        private readonly IRpcMerCollect _RpcMer;
        private readonly IRpcServerCollect _RpcServer;
        public ClientLimitService (IServerClientLimitCollect limit,
            IRpcServerCollect rpcServer,
            IRpcMerCollect rpcMer,
            IRemoteGroupCollect remoteGroup)
        {
            this._RpcMer = rpcMer;
            this._RpcServer = rpcServer;
            this._Limit = limit;
            this._RemoteGroup = remoteGroup;
        }
        public void Delete (long id)
        {
            ServerClientLimitModel limit = this._Limit.Get(id);
            this._Limit.Delete(limit);
            this._RpcServer.RefreshClientLimit(limit.RpcMerId, limit.ServerId);
        }

        public ClientLimitData Get (long rpcMerId, long serverId)
        {
            ServerClientLimitModel limit = this._Limit.Get(rpcMerId, serverId);
            if (limit == null)
            {
                return null;
            }
            return limit.ConvertMap<ServerClientLimitModel, ClientLimitData>();
        }

        public ClientLimitModel[] GetClientLimit (long serverId)
        {
            long[] rpcMerId = this._RemoteGroup.GetRpcMerId(serverId);
            if (rpcMerId.IsNull())
            {
                return new ClientLimitModel[0];
            }
            ServerClientLimitModel[] limit = this._Limit.Gets(rpcMerId, serverId);
            Dictionary<long, string> rpcMer = this._RpcMer.GetNames(rpcMerId);
            return rpcMerId.ConvertAll(c =>
            {
                return new ClientLimitModel
                {
                    RpcMerId = c,
                    RpcMerName = rpcMer.GetValueOrDefault(c),
                    Limit = limit.ConvertFind<ServerClientLimitModel, ClientLimitData>(a => a.RpcMerId == c)
                };
            });
        }

        public void Sync (ClientLimitDatum add)
        {
            if (this._Limit.Sync(add))
            {
                this._RpcServer.RefreshClientLimit(add.RpcMerId, add.ServerId);
            }
        }
    }
}
