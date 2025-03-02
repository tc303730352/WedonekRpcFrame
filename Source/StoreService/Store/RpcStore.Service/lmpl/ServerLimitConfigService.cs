using WeDonekRpc.Client;
using RpcManageClient;
using WeDonekRpc.Model.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.LimitConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerLimitConfigService : IServerLimitConfigService
    {
        private readonly IServerLimitConfigCollect _LimitConfig;
        private IRpcServerCollect _RpcServer;
        public ServerLimitConfigService(IServerLimitConfigCollect config,
            IRpcServerCollect rpcServer)
        {
            _RpcServer = rpcServer;
            _LimitConfig = config;
        }

        public void Delete(long serverId)
        {
            ServerLimitConfigModel limit = _LimitConfig.Get(serverId);
            _LimitConfig.Delete(limit);
            _RpcServer.RefreshLimit(serverId);
        }

        public ServerLimitConfig GetLimitConfig(long serverId)
        {
            ServerLimitConfigModel limit = _LimitConfig.Get(serverId, true);
            if (limit == null)
            {
                return new ServerLimitConfig { IsEnable = false };
            }
            return limit.ConvertMap<ServerLimitConfigModel, ServerLimitConfig>();
        }

        public void SyncConfig(LimitConfigDatum config)
        {
            if (_LimitConfig.SyncConfig(config))
            {
                _RpcServer.RefreshLimit(config.ServerId);
            }
        }
    }
}
