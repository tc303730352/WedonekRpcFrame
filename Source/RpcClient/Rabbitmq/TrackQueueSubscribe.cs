using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Track.Model;

using RpcModel;

namespace RpcClient.Rabbitmq
{
        [Attr.IgnoreIoc]
        internal class TrackQueueSubscribe : IQueueSubscribe
        {
                private readonly IQueueSubscribe _Subscribe = null;
                private readonly SubscribeEvent _Action = null;
                public TrackQueueSubscribe(RabbitmqQueue rabbitmq, RabbitmqConfig config, SubscribeEvent action)
                {
                        this._Action = action;
                        this._Subscribe = new RabbitmqSubscribe(rabbitmq, config, this._SubEvent);
                }
                private bool _SubEvent(QueueRemoteMsg msg, string routeKey, string exchange)
                {
                        if (msg.Msg.Track == null)
                        {
                                return this._Action(msg, routeKey, exchange);
                        }
                        using (TrackBody body = QueueTrackCollect.CreateAnswerTrack(msg, routeKey, exchange))
                        {
                                try
                                {
                                        bool isSuccess = this._Action(msg, routeKey, exchange);
                                        QueueTrackCollect.EndTrack(body, isSuccess);
                                        return isSuccess;
                                }
                                catch
                                {
                                        QueueTrackCollect.EndTrack(body, false);
                                        throw;
                                }
                        }
                }
                public void BindRoute(string routeKey)
                {
                        this._Subscribe.BindRoute(routeKey);
                }

                public void Dispose()
                {
                        this._Subscribe.Dispose();
                }

                public void Subscrib()
                {
                        this._Subscribe.Subscrib();
                }
        }
}
