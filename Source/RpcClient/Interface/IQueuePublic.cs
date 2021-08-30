using RpcModel;

namespace RpcClient.Interface
{
        public interface IQueuePublic
        {
                bool Public(string[] routeKey, string exchange, QueueRemoteMsg msg);
        }
}