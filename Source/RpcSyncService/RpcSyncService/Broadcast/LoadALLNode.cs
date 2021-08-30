using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        /// <summary>
        /// 加载区域所有节点
        /// </summary>
        internal class LoadALLNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return msg.IsCrossGroup && !msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
                }
                public bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body, out string error)
                {
                        if (!RemoteServerCollect.GetAllServer(msg.RegionId, out long[] serverId, out error))
                        {
                                return false;
                        }
                        else
                        {
                                body.ServerId = serverId;
                                error = null;
                                return true;
                        }
                }
        }
}
