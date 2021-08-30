using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Controller;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        /// <summary>
        /// 加载集群下的所有节点
        /// </summary>
        internal class LoadALLMerNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return !msg.IsCrossGroup && !msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
                }

                public bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body, out string error)
                {
                        long rpcMerId = msg.RpcMerId == 0 ? source.RpcMerId : msg.RpcMerId;
                        if (!RpcMerCollect.GetRpcMer(rpcMerId, out RpcMerController mer))
                        {
                                error = mer.Error;
                                return false;
                        }
                        else
                        {
                                body.RpcMerId = rpcMerId;
                                body.ServerId = mer.GetAllServer(msg.RegionId);
                                error = null;
                                return true;
                        }
                }
        }
}
