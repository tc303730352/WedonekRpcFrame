using RpcModel;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Broadcast
{
        internal class LoadServerNode : IInitBroadcast
        {
                public bool CheckIsUsable(BroadcastMsg msg)
                {
                        return !msg.ServerId.IsNull();
                }

                public bool InitBroadcastBody(BroadcastMsg msg, MsgSource source, ref BroadcastBody body, out string error)
                {
                        body.ServerId = msg.ServerId;
                        error = null;
                        return true;
                }
        }
}
