using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Queue.Model;
using WeDonekRpc.Client.Rabbitmq;
using WeDonekRpc.Client.Rabbitmq.Interface;
using WeDonekRpc.Client.Rabbitmq.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// 死信队列服务
    /// </summary>
    [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.SingleInstance)]
    internal class DeadQueueCollect : IDeadQueueCollect
    {
        private IDeadSubscribe _Subscribe;
        private DeadMsg _MsgEvent;

        private void _Callback (DeadQueueMsg msg, string routeKey, BroadcastType type)
        {
            this._MsgEvent(msg, routeKey, type);
        }

        public void BindQueue (string queue, DeadMsg callback)
        {
            if (this._Subscribe == null)
            {
                throw new ErrorException("rpc.rabbitmq.no.config");
            }
            this._Subscribe.BindQueue(queue);
            this._MsgEvent += callback;
        }
        public bool InitQueue ()
        {
            RabbitmqConfig config = Config.WebConfig.GetRabbitmqConfig();
            if (config != null)
            {
                RabbitmqQueue rabbitmq = new RabbitmqQueue(config);
                this._Subscribe = new DeadSubscribe(rabbitmq, RpcStateCollect.LocalSource, this._Callback);
                return true;
            }
            return false;
        }
    }
}
