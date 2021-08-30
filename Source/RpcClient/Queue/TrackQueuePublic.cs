using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Track.Model;

using RpcModel;

namespace RpcClient.Queue
{
        [Attr.IgnoreIoc]
        internal class TrackQueuePublic : IQueuePublic
        {
                private readonly IQueuePublic _Public = null;
                public TrackQueuePublic(IQueuePublic qp)
                {
                        this._Public = qp;
                }

                public bool Public(string[] routeKey, string exchange, QueueRemoteMsg msg)
                {
                        if (!QueueTrackCollect.CheckIsTrace(out long traceId))
                        {
                                return this._Public.Public(routeKey, exchange, msg);
                        }
                        using (TrackBody body = QueueTrackCollect.CreateTrack(traceId, routeKey, exchange, msg))
                        {
                                msg.Msg.Track = body.Trace;
                                bool isSuccess = this._Public.Public(routeKey, exchange, msg);
                                QueueTrackCollect.EndTrack(body, isSuccess);
                                return isSuccess;
                        }
                }
        }
}
