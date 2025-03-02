using WeDonekRpc.Model;
using RpcSync.Collect.Model;

namespace RpcSync.Collect
{
    public interface IBroadcastErrorCollect
    {
        void AddErrorLog (BroadcastBody msg, string error);
        void AddErrorLog (QueueRemoteMsg msg, string routeKey, BroadcastType msgType);
        void AddErrorLog (RemoteBroadcast data, string error);
    }
}