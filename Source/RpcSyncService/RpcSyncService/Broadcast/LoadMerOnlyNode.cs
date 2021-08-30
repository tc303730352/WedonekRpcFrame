using System.Collections.Generic;

using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Controller;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        internal class LoadMerOnlyNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return !msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && !msg.TypeVal.IsNull();
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
                                List<RootNode> nodes = new List<RootNode>();
                                msg.TypeVal.ForEach(a => DictateNodeCollect.Load(a, nodes));
                                if (nodes.Count == 0)
                                {
                                        error = "rpc.sync.node.null";
                                        return false;
                                }
                                body.RpcMerId = rpcMerId;
                                body.Dictate = mer.GetDictate(nodes, msg.RegionId);
                                error = null;
                                return true;
                        }
                }
        }
}
