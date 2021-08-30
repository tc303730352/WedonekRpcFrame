using RpcClient.Collect;

using RpcClient.Interface;

using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Collect
{
        internal class BroadcastCollect
        {
                private static readonly DataQueueHelper<RemoteBroadcast> _BroadcastQueue = null;
                private static readonly ITrackCollect _Track = null;
                static BroadcastCollect()
                {
                        _Track = RpcClient.RpcClient.Unity.Resolve<ITrackCollect>();
                        _BroadcastQueue = new DataQueueHelper<RemoteBroadcast>(new SyncQueueData<RemoteBroadcast>(_Send), new SaveQueueFailLog<RemoteBroadcast>(_AddErrorLog), 100, 50);
                }
                private static void _AddErrorLog(RemoteBroadcast data, string error, int retryNum)
                {
                        BroadcastErrorCollect.AddErrorLog(data, error);
                }
                private static void _Send(RemoteBroadcast obj)
                {
                        if (!_Send(obj, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }
                private static bool _Send(RemoteBroadcast obj, out string error)
                {
                        BroadcastBody msg = obj.Msg;
                        if (msg.Track != null)
                        {
                                _Track.SetTrack(msg.Track);
                        }
                        if (obj.ServerId == 0)
                        {
                                return RemoteCollect.Send(obj.TypeVal, msg.Datum, msg.Source, out error);
                        }
                        else
                        {
                                return RemoteCollect.Send(obj.ServerId, msg.Datum, msg.Source, out error);
                        }
                }

                internal static void SendBroadcast(BroadcastBody body)
                {
                        if (!body.ServerId.IsNull())
                        {
                                body.ServerId.ForEach(a =>
                                {
                                        if (!body.IsExclude || a != body.Source.SourceServerId)
                                        {
                                                _BroadcastQueue.AddQueue(new RemoteBroadcast
                                                {
                                                        Msg = body,
                                                        ServerId = a
                                                });
                                        }
                                });
                        }
                        else if (!body.Dictate.IsNull())
                        {
                                body.Dictate.ForEach(a =>
                                {
                                        if (!body.IsExclude || a != body.Source.SystemType)
                                        {
                                                _BroadcastQueue.AddQueue(new RemoteBroadcast
                                                {
                                                        Msg = body,
                                                        TypeVal = a
                                                });
                                        }
                                });
                        }
                        else
                        {
                                BroadcastErrorCollect.AddErrorLog(body, "rpc.sync.node.null");
                        }
                }
        }
}
