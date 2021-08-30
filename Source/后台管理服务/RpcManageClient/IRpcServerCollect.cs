using RpcManageClient.Model;

namespace RpcManageClient
{
        public interface IRpcServerCollect
        {
                void RefreshClientLimit(long rpcMerId, long serverId);
                void RefreshDictateLimit(long serverId, string dictate);
                void RefreshMer(long merId);
                void RefreshReduce(long rpcMerId, long serverId);
                void RefreshConfig(long merId, long systypeId);

                void RefreshLimit(long serverId);
                void RefreshMerConfig(long rpcMerId, long systemTypeId);
                void RefreshService(ServiceDatum service);
        }
}