using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track.Model;

using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Rabbitmq
{
    [Attr.IgnoreIoc]
    internal class TrackQueueSubscribe : IQueueSubscribe
    {
        private readonly IQueueSubscribe _Subscribe = null;
        private readonly SubscribeEvent _Action = null;
        public TrackQueueSubscribe (RabbitmqQueue rabbitmq, RabbitmqSet config, SubscribeEvent action)
        {
            this._Action = action;
            this._Subscribe = new RabbitmqSubscribe(rabbitmq, config, this._SubEvent);
        }
        private SubscribeErrorType _SubEvent (QueueRemoteMsg msg, string routeKey, string exchange)
        {
            if (msg.Msg.Track == null)
            {
                return this._Action(msg, routeKey, exchange);
            }
            using (TrackBody body = QueueTrackCollect.CreateAnswerTrack(msg, routeKey, exchange))
            {
                try
                {
                    SubscribeErrorType res = this._Action(msg, routeKey, exchange);
                    body.Commit(res == SubscribeErrorType.Success);
                    return res;
                }
                catch
                {
                    body.Commit(false);
                    throw;
                }
            }
        }
        public void BindRoute (string routeKey)
        {
            this._Subscribe.BindRoute(routeKey);
        }

        public void Dispose ()
        {
            this._Subscribe.Dispose();
        }

        public void Subscrib ()
        {
            this._Subscribe.Subscrib();
        }
    }
}
