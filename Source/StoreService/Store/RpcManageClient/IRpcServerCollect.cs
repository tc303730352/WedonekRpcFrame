using RpcManageClient.Model;

namespace RpcManageClient
{
    public interface IRpcServerCollect
    {
        void RefreshTransmit (long rpcMerId, long sysTypeId);
        void RefreshClientLimit (long rpcMerId, long serverId);
        void RefreshDictateLimit (long serverId, string dictate);
        void RefreshMer (long merId);
        void RefreshReduce (long rpcMerId, long serverId);
        void RefreshConfig (long merId, long systypeId);

        void RefreshVerNum (long serverId, int verNum, int oldVerNum);
        void RefreshVerConfig (long merId, long sysTypeId, long verNum);
        void RefreshLimit (long serverId);
        void RefreshMerConfig (long rpcMerId, long systemTypeId);
        void RefreshService (ServiceDatum service);

        void RefreshControl (int regionId);

        void RefreshVerNum (long rpcMerId, long systemTypeId, int verNum);
    }
}