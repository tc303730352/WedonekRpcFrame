using RpcSync.Collect.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Model;

namespace RpcSync.Service.Broadcast
{
    [IocName("Server")]
    internal class LoadServerNode : IInitBroadcast
    {

        public void InitBroadcastBody (BroadcastMsg msg, MsgSource source, ref BroadcastBody body)
        {
            body.ServerId = msg.ServerId;
        }
    }
}
