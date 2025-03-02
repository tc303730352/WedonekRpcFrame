using WeDonekRpc.Model;
using RpcSync.Collect.Model;

namespace RpcSync.Service.Interface
{
    public interface ILoadBroadcast
    {
        BroadcastBody InitBroadcastBody(BroadcastMsg msg, MsgSource source);
    }
}