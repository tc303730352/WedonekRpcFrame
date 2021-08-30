using RpcModel.Model;

using Wedonek.RpcStore.Gateway.Interface;
using Wedonek.RpcStore.Service.Interface;
using Wedonek.RpcStore.Service.Model;

namespace Wedonek.RpcStore.Gateway.Service
{
        internal class ServerLimitConfigService : IServerLimitConfigService
        {
                private readonly IServerLimitConfigCollect _LimitConfig = null;

                public ServerLimitConfigService(IServerLimitConfigCollect config)
                {
                        this._LimitConfig = config;
                }

                public void DropConfig(long serverId)
                {
                        this._LimitConfig.DropConfig(serverId);
                }

                public ServerLimitConfig GetLimitConfig(long serverId)
                {
                        return this._LimitConfig.GetLimitConfig(serverId);
                }

                public void SyncConfig(AddServerLimitConfig config)
                {
                        this._LimitConfig.SyncConfig(config);
                }
        }
}
