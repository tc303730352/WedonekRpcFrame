using System;
using System.Linq;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    internal class QueueTrackCollect
    {
        private static readonly ITrackCollect _Track = new TrackCollect();

        /// <summary>
        /// 获取接收的参数
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="exchange"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static TrackArg[] _GetAnswerArgs (string routeKey, string exchange, QueueRemoteMsg msg)
        {
            TrackDepth[] depth = _Track.GetAnswerTemplate();
            if (depth.Length == 0 || !depth.IsExists(a => a == TrackDepth.接收的数据))
            {
                return new TrackArg[]
                {
                    new TrackArg
                    {
                            Key="Result"
                    },
                    new TrackArg
                    {
                            Key="MsgType",
                            Value="Queue"
                    },
                    new TrackArg
                    {
                            Key="Exchange",
                            Value=exchange
                    },
                        new TrackArg
                    {
                            Key="RouteKey",
                            Value=routeKey
                    }
                };
            }
            return new TrackArg[]
            {
                    new TrackArg
                    {
                            Key="Result"
                    },
                    new TrackArg
                    {
                            Key = "Param",
                            Value = msg.Msg.MsgBody
                    },
                    new TrackArg
                    {
                            Key="MsgType",
                            Value="Queue"
                    },
                    new TrackArg
                    {
                            Key="Exchange",
                            Value=exchange
                    },
                            new TrackArg
                    {
                            Key="RouteKey",
                            Value=routeKey
                    }
            };
        }

        internal static bool CheckIsTrace (out long traceId)
        {
            return _Track.CheckIsTrack(TrackRange.RpcQueue, out traceId);
        }

        /// <summary>
        /// 获取发送的参数
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="exchange"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static TrackArg[] _GetArgs (string[] routeKey, string exchange, QueueRemoteMsg msg)
        {
            TrackDepth[] depth = _Track.GetArgTemplate();
            if (depth.Length == 0 || !depth.IsExists(a => a == TrackDepth.发起的参数))
            {
                return new TrackArg[]
                {
                    new TrackArg
                    {
                            Key="Result"
                    },
                    new TrackArg
                    {
                            Key="MsgType",
                            Value="Queue"
                    },
                    new TrackArg
                    {
                            Key="Exchange",
                            Value=exchange
                    },
                        new TrackArg
                    {
                            Key="RouteKey",
                            Value=routeKey.Join(",")
                    },
                    new TrackArg
                    {
                            Key="ServerId",
                            Value=msg.Msg.Source.ServerId.ToString(),
                    },
                    new TrackArg
                    {
                            Key="SystemType",
                            Value=msg.Msg.Source.SystemType
                    }
                };
            }
            return new TrackArg[]
            {
                    new TrackArg
                    {
                            Key="Result"
                    },
                    new TrackArg
                    {
                            Key="MsgType",
                            Value="Queue"
                    }, new TrackArg
                    {
                            Key = "Param",
                            Value = msg.Msg.MsgBody
                    },
                            new TrackArg
                    {
                            Key="Exchange",
                            Value=exchange
                    },
                            new TrackArg
                    {
                            Key="RouteKey",
                            Value=routeKey.Join(",")
                    },
                    new TrackArg
                    {
                            Key="ServerId",
                            Value=msg.Msg.Source.ServerId.ToString(),
                    },
                    new TrackArg
                    {
                            Key="SystemType",
                            Value=msg.Msg.Source.SystemType
                    }
            };
        }
        /// <summary>
        /// 创建请求链路
        /// </summary>
        /// <param name="spanId"></param>
        /// <param name="routeKey"></param>
        /// <param name="exchange"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static TrackBody CreateTrack (long spanId, string[] routeKey, string exchange, QueueRemoteMsg msg)
        {
            DateTime now = DateTime.Now;
            return new TrackBody
            {
                Dictate = msg.Type,
                StageType = StageType.Send,
                Trace = _Track.CreateTrack(spanId),
                Port = 0,
                RemoteIp = "0.0.0.0",
                Time = now,
                Args = _GetArgs(routeKey, exchange, msg),
                Annotations = new TrackAnnotation[]
                {
                    new TrackAnnotation
                    {
                            Stage= TrackStage.cs,
                            Time=now
                    }
                    ,new TrackAnnotation
                    {
                            Stage= TrackStage.cr
                    }
                }
            };
        }
        /// <summary>
        /// 创建应答链路
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="exchange"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static TrackBody CreateAnswerTrack (QueueRemoteMsg msg, string routeKey, string exchange)
        {
            DateTime now = DateTime.Now;
            return new TrackBody
            {
                Dictate = msg.Type,
                StageType = StageType.Answer,
                Trace = _Track.CreateAnswerTrack(msg.Msg.Track),
                Port = 0,
                RemoteIp = "0.0.0.0",
                Time = now,
                Args = _GetAnswerArgs(routeKey, exchange, msg),
                Annotations = new TrackAnnotation[]
                {
                    new TrackAnnotation
                    {
                            Stage= TrackStage.sr,
                            Time=now
                    }
                    ,new TrackAnnotation
                    {
                            Stage= TrackStage.ss
                    }
                }
            };
        }
        internal static void EndTrack (TrackBody track, bool isSuccess)
        {
            track.Args[0].Value = isSuccess ? "OK" : "Error";
            _Track.EndTrack(track);
        }
    }
}
