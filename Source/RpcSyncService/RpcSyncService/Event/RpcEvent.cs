using RpcModel;

using RpcSyncService.Logic;

namespace RpcSyncService.Event
{
        internal class RpcEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 广播消息
                /// </summary>
                /// <param name="msg">消息</param>
                /// <param name="source">数据源</param>
                public static void Broadcast(BroadcastMsg msg, MsgSource source)
                {
                        BroadcastLogic.SendBroadcast(msg, source);
                }

        }
}
