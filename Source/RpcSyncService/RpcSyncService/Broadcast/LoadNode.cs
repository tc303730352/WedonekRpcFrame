using System.Collections.Generic;

using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        internal class LoadNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return msg.IsCrossGroup && msg.IsLimitOnly == false && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
                }

                public bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body, out string error)
                {
                        List<RootNode> nodes = new List<RootNode>();
                        msg.TypeVal.ForEach(a => DictateNodeCollect.Load(a, nodes));
                        if (nodes.Count == 0)
                        {
                                error = "rpc.sync.node.null";
                                return false;
                        }
                        else if (!RemoteServerCollect.GetServerId(nodes, msg.RegionId, out long[] serverId, out error))
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
