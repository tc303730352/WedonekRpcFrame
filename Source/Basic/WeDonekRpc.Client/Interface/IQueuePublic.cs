using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Interface
{
    public interface IQueuePublic
    {
        bool Public(string[] routeKey, string exchange, QueueRemoteMsg msg);
        bool PublicTran(string[] routeKey, string exchange, QueueRemoteMsg msg);
    }
}