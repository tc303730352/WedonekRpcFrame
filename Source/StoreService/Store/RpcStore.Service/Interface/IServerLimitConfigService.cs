using WeDonekRpc.Model.Model;
using RpcStore.RemoteModel.LimitConfig.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerLimitConfigService
    {
        void Delete(long serverId);
        ServerLimitConfig GetLimitConfig(long serverId);
        void SyncConfig(LimitConfigDatum config);
    }
}