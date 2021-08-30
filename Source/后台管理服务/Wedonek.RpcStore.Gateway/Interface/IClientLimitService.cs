using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IClientLimitService
        {
                void Drop(long rpcMerId, long serverId);
                ServerClientLimitData Get(long rpcMerId, long serverId);
                void Sync(ServerClientLimitData add);
        }
}