using RpcClient.Queue.Model;
using RpcClient.Rabbitmq.Model;

namespace RpcClient.Rabbitmq
{
        internal class RabbitmqConfig
        {
                public QueueConfig Config
                {
                        get;
                        set;
                }
                public SubscribBody[] Subscribs
                {
                        get;
                        set;
                }
                public bool IsAutoAck
                {
                        get;
                        set;
                }
        }
}
