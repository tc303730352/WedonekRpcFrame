using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
namespace RpcSync.Service.Event
{
    internal class RpcBroadcastEvent : IRpcApiService
    {
        private readonly IBroadcastService _Broadcast;
        public RpcBroadcastEvent (IBroadcastService broadcast)
        {
            this._Broadcast = broadcast;
        }


        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="source">数据源</param>
        public void Broadcast (BroadcastMsg msg, MsgSource source)
        {
            this._Broadcast.Send(msg, source);
        }

    }
}
