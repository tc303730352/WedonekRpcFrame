using WeDonekRpc.Model.Model;
using RpcStore.RemoteModel.LimitConfig;
using RpcStore.RemoteModel.LimitConfig.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class LimitConfigService : ILimitConfigService
    {
        public void DeleteLimitConfig(long serverId)
        {
            new DeleteLimitConfig
            {
                ServerId = serverId,
            }.Send();
        }

        public ServerLimitConfig GetLimitConfig(long serverId)
        {
            return new GetLimitConfig
            {
                ServerId = serverId,
            }.Send();
        }

        public void SyncLimitConfig(LimitConfigDatum config)
        {
            new SyncLimitConfig
            {
                Config = config,
            }.Send();
        }

    }
}
