using RpcModel;

using RpcSyncService.Broadcast;
using RpcSyncService.Collect;
using RpcSyncService.Model;
namespace RpcSyncService.Logic
{
        internal class BroadcastLogic
        {
                public static void SendBroadcast(BroadcastMsg msg, MsgSource source)
                {
                        if (!InitBroadcast.InitBroadcastBody(msg, source, out BroadcastBody body, out string error))
                        {
                                BroadcastErrorCollect.AddErrorLog(msg, source, error);
                                return;
                        }
                        BroadcastCollect.SendBroadcast(body);
                }

        }
}
