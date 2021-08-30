using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Controller;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        /// <summary>
        /// 加载当前集群下的所有指令集
        /// </summary>
        internal class LoadALLMerOnlyNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return !msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
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
                                body.Dictate = mer.GetAllDictate(msg.RegionId);
                                error = null;
                                return true;
                        }
                }
        }
}
