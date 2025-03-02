using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Rabbitmq
{
    [Attr.IgnoreIoc]
    internal class QueueRabbitmq : IQueue
    {
        private readonly RabbitmqQueue _Rabbitmq = null;
        private readonly IQueuePublic _Public = null;
        private readonly IQueueSubscribe _Subscribe = null;

        public QueueRabbitmq (RabbitmqSet config, SubscribeEvent action)
        {
            this._Rabbitmq = new RabbitmqQueue(config.Config);
            if (TrackCollect.IsEnable)
            {
                this._Public = new Queue.TrackQueuePublic(new RabbitmqPublic(this._Rabbitmq));
                this._Subscribe = new TrackQueueSubscribe(this._Rabbitmq, config, action);
            }
            else
            {
                this._Public = new RabbitmqPublic(this._Rabbitmq);
                this._Subscribe = new RabbitmqSubscribe(this._Rabbitmq, config, action);
            }
        }

        public void Subscrib ()
        {
            this._Subscribe.Subscrib();
        }
        public void BindRoute (string routeKey)
        {
            this._Subscribe.BindRoute(routeKey);
        }

        public void Dispose ()
        {
            this._Subscribe.Dispose();
            this._Rabbitmq.Dispose();
        }

        public bool Public (QueueRemoteMsg msg, string[] routeKey, string exchange)
        {
            if (!msg.TranId.HasValue)
            {
                return this._Public.Public(routeKey, exchange, msg);
            }
            return this._Public.PublicTran(routeKey, exchange, msg);
        }
    }
}
