using RpcModel.Model;

using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Interface
{
        internal interface IServerLimitConfigService
        {
                void DropConfig(long serverId);
                ServerLimitConfig GetLimitConfig(long serverId);
                void SyncConfig(AddServerLimitConfig config);
        }
}