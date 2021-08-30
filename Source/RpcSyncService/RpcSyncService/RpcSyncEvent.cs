using RpcClient.Interface;

using RpcModel;

using RpcSyncService.Collect;

namespace RpcSyncService
{
        /// <summary>
        /// RPC系统事件
        /// </summary>
        internal class RpcSyncEvent : IRpcEvent
        {
                public void RefreshService(long serverId)
                {
                        RemoteServerCollect.Refresh(serverId);
                }

                public void ServerClose()
                {
                }

                public void ServerStarted()
                {
                }

                public void ServerStarting()
                {
                }

                public void ServiceState(RpcServiceState state)
                {
                }
        }
}
