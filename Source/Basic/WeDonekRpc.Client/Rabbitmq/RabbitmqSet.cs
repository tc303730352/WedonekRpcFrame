using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq.Model;

namespace WeDonekRpc.Client.Rabbitmq
{
        internal class RabbitmqSet
        {
                public RabbitmqConfig Config
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
