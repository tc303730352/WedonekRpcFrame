using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerClientLimitCollect
        {
                void DropConfig(long rpcMerId, long serverId);
                ServerClientLimitData GetConfig(long rpcMerId, long serverId);
                void Sync(ServerClientLimitData add);
        }
}