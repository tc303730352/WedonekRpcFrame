using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ClientLimitService : IClientLimitService
        {
                private readonly IServerClientLimitCollect _Limit = null;

                public ClientLimitService(IServerClientLimitCollect limit)
                {
                        this._Limit = limit;
                }
                public void Drop(long rpcMerId, long serverId)
                {
                        this._Limit.DropConfig(rpcMerId, serverId);
                }

                public ServerClientLimitData Get(long rpcMerId, long serverId)
                {
                        return this._Limit.GetConfig(rpcMerId, serverId);
                }

                public void Sync(ServerClientLimitData add)
                {
                        this._Limit.Sync(add);
                }
        }
}
