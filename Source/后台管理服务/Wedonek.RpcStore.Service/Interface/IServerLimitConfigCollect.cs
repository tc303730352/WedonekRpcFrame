using RpcModel.Model;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Service.Interface
{
        public interface IServerLimitConfigCollect
        {
                void DropConfig(long serverId);
                ServerLimitConfig GetLimitConfig(long serverId);
                void SyncConfig(AddServerLimitConfig config);
        }
}