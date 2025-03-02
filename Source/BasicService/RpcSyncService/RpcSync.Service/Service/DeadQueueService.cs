using RpcSync.Collect;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;

namespace RpcSync.Service.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class DeadQueueService : IDeadQueueService
    {
        private readonly IDeadQueueCollect _DeadQuery;
        private readonly IBroadcastErrorCollect _Error;
        public DeadQueueService ( IDeadQueueCollect deadQuery, IBroadcastErrorCollect error )
        {
            this._Error = error;
            this._DeadQuery = deadQuery;
        }

        public void Init ()
        {
            if ( this._DeadQuery.InitQueue() )
            {
                this._DeadQuery.BindQueue("Rpc_Dead_Queue", this._DeadMsg);
            }
        }

        private void _DeadMsg ( DeadQueueMsg msg, string routeKey, BroadcastType msgType )
        {
            if ( msg.Msg == null || msg.Msg.Msg == null )
            {
                new WarnLog("rpc.queue.msg.format.error", "广播消息格式错误!", "Rpc_Broadcast")
                                {
                                        { "RouteKey",routeKey},
                                        { "Msg",msg.MsgBody},
                                        { "BroadcastType",msgType}
                                }.Save();
                return;
            }
            this._Error.AddErrorLog(msg.Msg, routeKey, msgType);
        }
    }
}
