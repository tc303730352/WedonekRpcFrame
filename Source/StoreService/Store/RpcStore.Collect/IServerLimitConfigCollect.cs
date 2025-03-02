using RpcStore.Model.DB;
using RpcStore.RemoteModel.LimitConfig.Model;

namespace RpcStore.Collect
{
    public interface IServerLimitConfigCollect
    {
        void Delete(ServerLimitConfigModel config);
        ServerLimitConfigModel Get(long serverId,bool isAllowNull=false);
        bool SyncConfig(LimitConfigDatum datum);
    }
}