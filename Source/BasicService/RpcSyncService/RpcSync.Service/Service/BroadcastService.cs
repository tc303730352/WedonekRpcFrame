using RpcSync.Collect;
using RpcSync.Collect.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcSync.Service.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class BroadcastService : IBroadcastService
    {
        private readonly DataQueueHelper<RemoteBroadcast> _BroadcastQueue = null;
        private readonly ITrackCollect _Track;
        private readonly IBroadcastErrorCollect _ErrorLog;
        private readonly ILoadBroadcast _InitBroadcast;
        public BroadcastService (ITrackCollect track,
            ILoadBroadcast load,
            IBroadcastErrorCollect errorLog)
        {
            this._InitBroadcast = load;
            this._ErrorLog = errorLog;
            this._Track = track;
            this._BroadcastQueue = new DataQueueHelper<RemoteBroadcast>(new SyncQueueData<RemoteBroadcast>(this._Send), new SaveQueueFailLog<RemoteBroadcast>(this._AddErrorLog), 100, 50);
        }
        private void _AddErrorLog (RemoteBroadcast data, string error, int retryNum)
        {
            this._ErrorLog.AddErrorLog(data, error);
        }
        private void _Send (RemoteBroadcast obj)
        {
            if (!this._Send(obj, out string error))
            {
                throw new ErrorException(error);
            }
        }
        private bool _Send (RemoteBroadcast obj, out string error)
        {
            BroadcastBody msg = obj.Msg;
            if (msg.Track != null)
            {
                this._Track.SetTrack(msg.Track);
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
        public void Send (BroadcastMsg msg, MsgSource source)
        {
            BroadcastBody body = this._InitBroadcast.InitBroadcastBody(msg, source);
            this._Send(body);
        }

        private void _Send (BroadcastBody body)
        {
            if (!body.ServerId.IsNull())
            {
                body.ServerId.ForEach(a =>
                {
                    if (!body.IsExclude || a != body.Source.ServerId)
                    {
                        this._BroadcastQueue.AddQueue(new RemoteBroadcast
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
                        this._BroadcastQueue.AddQueue(new RemoteBroadcast
                        {
                            Msg = body,
                            TypeVal = a
                        });
                    }
                });
            }
            else
            {
                this._ErrorLog.AddErrorLog(body, "rpc.sync.node.null");
            }
        }
    }
}
