using RpcModel;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        /// <summary>
        /// 加载所有根节点
        /// </summary>
        internal class LoadALLOnlyNode : IInitBroadcast
        {

                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return msg.IsCrossGroup && msg.IsLimitOnly && msg.ServerId.IsNull() && msg.TypeVal.IsNull();
                }
                public bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body, out string error)
                {
                        RootNode[] nodes = DictateNodeCollect.GetRootNode();
                        body.Dictate = nodes.ConvertAll(a => a.Dictate);
                        error = null;
                        return true;
                }
        }
}
