using WeDonekRpc.Model;

namespace RpcSync.Service.Interface
{
    public interface IBroadcastService
    {
        void Send(BroadcastMsg msg, MsgSource source);
    }
}