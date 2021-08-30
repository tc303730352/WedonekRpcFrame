using System.Collections.Generic;
using System.Linq;

using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        internal class LoadOnlyNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
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
                        else
                        {
                                body.Dictate = nodes.Select(a => a.Dictate).Distinct().ToArray();
                                error = null;
                                return true;
                        }
                }
        }
}
