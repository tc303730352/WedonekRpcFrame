using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Rabbitmq.Model
{
        public class DeadQueueMsg
        {
                public string MsgBody
                {
                        get;
                        set;
                }
                public QueueRemoteMsg Msg
                {
                        get;
                        set;
                }
        }
}
