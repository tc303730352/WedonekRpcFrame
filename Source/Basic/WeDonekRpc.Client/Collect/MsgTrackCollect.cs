using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.Model;

namespace WeDonekRpc.Client.Collect
{
    internal class MsgTrackCollect
    {
        private static readonly ITrackCollect _Track = null;
        private static readonly string _LocalId = RpcStateCollect.ServerId.ToString();
        private static readonly string _LocalName = RpcStateCollect.ServerConfig.Name;

        static MsgTrackCollect ()
        {
            _Track = RpcClient.Ioc.Resolve<ITrackCollect>();
        }
        /// <summary>
        /// 获取接收的参数
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static TrackArg[] _GetAnswerArgs (TcpRemoteMsg msg, string clientName, string mode = "tcp")
        {
            TrackDepth[] depth = _Track.GetAnswerTemplate();
            if (depth.Length == 0)
            {
                return new TrackArg[]
                {
                       new TrackArg
                        {
                                Key="Result"
                        },
                       new TrackArg
                        {
                                Key="PageId",
                                Value=msg.PageId.ToString(),
                        },
                       new TrackArg
                        {
                                Key="MsgType",
                                Value="RpcMsg"
                        },
                       new TrackArg
                        {
                                Key="MsgMode",
                                Value=mode
                        },
                       new TrackArg
                        {
                                Key="ClientName",
                                Value=clientName
                        },
                       new TrackArg
                        {
                                Key="SystemType",
                                Value=RpcStateCollect.LocalConfig.SystemType
                        },
                       new TrackArg
                       {
                                Key="LocalId",
                                Value=_LocalId
                       }
                };
            }
            List<TrackArg> args = new List<TrackArg>(depth.Length + 7)
            {
                new TrackArg
                {
                    Key = "Result"
                },
                new TrackArg
                {
                    Key = "PageId",
                    Value = msg.PageId.ToString()
                },
                new TrackArg
                {
                    Key = "MsgType",
                    Value = "RpcMsg"
                },
                new TrackArg
                {
                    Key = "MsgMode",
                    Value = mode
                },
                new TrackArg
                {
                    Key = "ClientName",
                    Value = clientName
                },
                new TrackArg
                {
                    Key = "SystemType",
                    Value = RpcStateCollect.LocalConfig.SystemType
                },
                new TrackArg
                {
                    Key = "LocalId",
                    Value = _LocalId
                }
            };
            depth.ForEach(a =>
           {
               if (a == TrackDepth.接收的数据)
               {
                   args.Add(new TrackArg
                   {
                       Key = "Param",
                       Value = msg.MsgBody
                   });
               }
               else
               {
                   args.Add(new TrackArg
                   {
                       Key = "Return"
                   });
               }
           });
            return args.ToArray();
        }

        /// <summary>
        /// 获取发送的参数
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static TrackArg[] _GetArgs (IRemote remote, TcpRemoteMsg msg)
        {
            TrackDepth[] depth = _Track.GetArgTemplate();
            if (depth.Length == 0)
            {
                return new TrackArg[]
                {
                    new TrackArg
                    {
                        Key="Result"
                    },
                    new TrackArg
                    {
                        Key="PageId",
                        Value=msg.PageId.ToString()
                    },
                    new TrackArg
                    {
                        Key="MsgType",
                        Value="RpcMsg"
                    },
                    new TrackArg
                    {
                        Key="LocalId",
                        Value=_LocalId
                    },
                    new TrackArg
                    {
                        Key="SystemType",
                        Value=RpcStateCollect.LocalConfig.SystemType
                    }
                };
            }
            List<TrackArg> args = new List<TrackArg>(depth.Length + 6)
            {
                new TrackArg
                {
                    Key = "Result"
                },
                new TrackArg
                {
                    Key = "PageId",
                    Value = msg.PageId.ToString()
                },
                new TrackArg
                {
                    Key = "MsgType",
                    Value = "RpcMsg"
                },
                new TrackArg
                {
                    Key = "RegionId",
                    Value = remote.RegionId.ToString()
                },
                new TrackArg
                {
                    Key = "LocalId",
                    Value = _LocalId
                },
                new TrackArg
                {
                    Key = "SystemType",
                    Value = RpcStateCollect.LocalConfig.SystemType
                }
            };
            depth.ForEach(a =>
            {
                if (a == TrackDepth.发起的参数)
                {
                    args.Add(new TrackArg
                    {
                        Key = "Param",
                        Value = msg.MsgBody
                    });
                }
                else
                {
                    args.Add(new TrackArg
                    {
                        Key = "Return"
                    });
                }
            });
            return args.ToArray();
        }
        /// <summary>
        /// 获取发送的参数
        /// </summary>
        /// <param name="remote"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private static TrackArg[] _GetArgs (IRemote remote, FileInfo file, TcpRemoteMsg msg)
        {
            TrackDepth[] depth = _Track.GetArgTemplate();
            if (depth.Length == 0)
            {
                return new TrackArg[]
                {
                    new TrackArg
                    {
                            Key="Result"
                    },
                    new TrackArg
                    {
                        Key = "PageId",
                        Value = msg.PageId.ToString()
                    },
                    new TrackArg
                    {
                            Key="MsgType",
                            Value="RpcMsg"
                    },
                    new TrackArg
                    {
                            Key="ServerId",
                            Value=remote.ServerId.ToString()
                    },
                    new TrackArg
                    {
                            Key="File",
                            Value=file.FullName
                    }
                };
            }
            List<TrackArg> args = new List<TrackArg>(depth.Length + 5)
            {
                new TrackArg
                {
                    Key = "Result"
                },
                new TrackArg
                {
                    Key = "PageId",
                    Value = msg.PageId.ToString()
                },
                new TrackArg
                {
                    Key = "MsgType",
                    Value = "RpcMsg"
                },
                new TrackArg
                {
                    Key = "ServerId",
                    Value = remote.ServerId.ToString()
                },
                new TrackArg
                {
                    Key = "File",
                    Value = file.FullName
                }
            };
            depth.ForEach(a =>
            {
                if (a == TrackDepth.发起的参数)
                {
                    args.Add(new TrackArg
                    {
                        Key = "Param",
                        Value = msg.MsgBody
                    });
                }
                else
                {
                    args.Add(new TrackArg
                    {
                        Key = "Return"
                    });
                }
            });
            return args.ToArray();
        }
        public static TrackBody CreateAnswerTrack (string type, TcpRemoteMsg msg, string clientName)
        {
            DateTime now = DateTime.Now;
            return new TrackBody
            {
                Dictate = type,
                StageType = StageType.Answer,
                Trace = _Track.CreateAnswerTrack(msg.Track),
                Port = 0,
                RemoteIp = "127.0.0.1",
                RemoteId = msg.Source.ServerId,
                Time = now,
                Args = _GetAnswerArgs(msg, "pipe", clientName),
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
        public static TrackBody CreateAnswerTrack (string type, TcpRemoteMsg msg, IPEndPoint clientIp)
        {
            DateTime now = DateTime.Now;
            return new TrackBody
            {
                Dictate = type,
                StageType = StageType.Answer,
                Trace = _Track.CreateAnswerTrack(msg.Track),
                Port = clientIp.Port,
                RemoteId = msg.Source.ServerId,
                RemoteIp = clientIp.Address.ToString(),
                Time = now,
                Args = _GetAnswerArgs(msg, string.Empty),
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
        public static TrackBody CreateTrack (long spanId, IRemoteConfig config, IRemote remote, TcpRemoteMsg msg)
        {
            DateTime now = DateTime.Now;
            return new TrackBody
            {
                Dictate = config.SysDictate,
                StageType = StageType.Send,
                Trace = _Track.CreateTrack(spanId),
                RemoteId = remote.ServerId,
                Port = remote.Port,
                RemoteIp = remote.ServerIp,
                Time = now,
                Args = _GetArgs(remote, msg),
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

        internal static bool CheckIsTrace (out long spanId)
        {
            return _Track.CheckIsTrack(TrackRange.RpcMsg, out spanId);
        }

        public static void EndTrack (TrackBody track, TcpRemoteReply reply)
        {
            track.Args[0].Value = "OK";
            if (track.Args[1].Key == "Return" && reply != null)
            {
                track.Args[1].Value = reply.MsgBody;
            }
            _Track.EndTrack(track);
        }

        public static void EndTrack (TrackBody track, string error)
        {
            track.Args[0].Value = error;
            _Track.EndTrack(track);
        }

        public static TrackBody CreateTrack (long tranId, IRemoteConfig config, IRemote remote, FileInfo file, TcpRemoteMsg msg)
        {
            DateTime now = DateTime.Now;
            return new TrackBody
            {
                Dictate = config.SysDictate,
                StageType = StageType.Send,
                Trace = _Track.CreateTrack(tranId),
                Port = remote.Port,
                RemoteIp = remote.ServerIp,
                RemoteId = remote.ServerId,
                Time = now,
                Args = _GetArgs(remote, file, msg),
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

        public static void EndTrack (TrackBody track, IFileUpResult result)
        {
            if (result.IsSuccess)
            {
                track.Args[0].Value = "ok";
                if (track.Args[1].Key == "Return")
                {
                    track.Args[1].Value = result.GetString();
                }
            }
            else
            {
                track.Args[0].Value = result.Error;
            }
            _Track.EndTrack(track);
        }
    }
}
