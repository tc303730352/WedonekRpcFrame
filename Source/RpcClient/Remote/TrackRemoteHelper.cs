using System.Collections.Generic;
using System.IO;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Track.Model;

using RpcModel;

using SocketTcpClient.UpFile;
namespace RpcClient.Track
{
        internal class TrackRemoteHelper : IRemote
        {
                private readonly IRemote _Remote = null;
                private readonly long _SpanId = 0;
                public TrackRemoteHelper(IRemote remote, long spanId)
                {
                        this._SpanId = spanId;
                        this._Remote = remote;
                }
                public string ServerIp => this._Remote.ServerIp;

                public int Port => this._Remote.Port;
                public bool IsUsable => this._Remote.IsUsable;

                public int OfflineTime => this._Remote.OfflineTime;

                public string ServerName => this._Remote.ServerName;
                public long SystemType => this._Remote.SystemType;

                public long ServerId => this._Remote.ServerId;

                public int RegionId => this._Remote.RegionId;

                public bool SendData(IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        using (TrackBody track = MsgTrackCollect.CreateTrack(this._SpanId, config, this._Remote, msg.MsgBody))
                        {
                                msg.Track = track.Trace;
                                if (this._Remote.SendData(config, msg, out reply, out error))
                                {
                                        MsgTrackCollect.EndTrack(track, reply);
                                        return true;
                                }
                                MsgTrackCollect.EndTrack(track, error);
                                return false;
                        }
                }

                public bool SendData(string dicate, IRemoteConfig config, TcpRemoteMsg msg, out TcpRemoteReply reply, out string error)
                {
                        using (TrackBody track = MsgTrackCollect.CreateTrack(this._SpanId, config, this._Remote, msg.MsgBody))
                        {
                                track.Dictate = dicate;
                                msg.Track = track.Trace;
                                if (this._Remote.SendData(dicate, config, msg, out reply, out error))
                                {
                                        MsgTrackCollect.EndTrack(track, reply);
                                        return true;
                                }
                                MsgTrackCollect.EndTrack(track, error);
                                return false;
                        }
                }

                public bool SendFile(IRemoteConfig config, TcpRemoteMsg msg, FileInfo file, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
                {
                        using (TrackBody track = MsgTrackCollect.CreateTrack(this._SpanId, config, this._Remote, file, msg.MsgBody))
                        {
                                msg.Track = track.Trace;
                                if (this._Remote.SendFile(config, msg, file, (a) =>
                                {
                                        MsgTrackCollect.EndTrack(track, a);
                                        if (func != null)
                                        {
                                                func(a);
                                        }
                                }, progress, out upTask, out error))
                                {
                                        return true;
                                }
                                MsgTrackCollect.EndTrack(track, error);
                        }
                        return false;
                }

                public Dictionary<string, dynamic> ToDictionary(int state)
                {
                        return this._Remote.ToDictionary(state);
                }

                public Dictionary<string, dynamic> ToDictionary()
                {
                        return this._Remote.ToDictionary();
                }
        }
}
