using System;
using System.IO;
using System.Net;

using RpcClient.Interface;
using RpcClient.Track;
using RpcClient.Track.Model;

using RpcModel;

using SocketTcpClient.UpFile.Model;

using RpcHelper;

namespace RpcClient.Collect
{
        internal class MsgTrackCollect
        {
                private static readonly ITrackCollect _Track = null;
                private static readonly string _LocalId = RpcStateCollect.ServerId.ToString();

                static MsgTrackCollect()
                {
                        _Track = RpcClient.Unity.Resolve<ITrackCollect>();
                }
                /// <summary>
                /// 获取接收的参数
                /// </summary>
                /// <param name="msg"></param>
                /// <returns></returns>
                private static TrackArg[] _GetAnswerArgs(TcpRemoteMsg msg)
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
                                                Key="MsgType",
                                                Value="RpcMsg"
                                        },
                                        new TrackArg
                                        {
                                                Key="SystemType",
                                                Value=RpcStateCollect.LocalSource.SystemType
                                        },
                                         new TrackArg
                                        {
                                                Key="LocalId",
                                                Value=_LocalId
                                        }
                                };
                        }
                        int index = 1;
                        TrackArg[] args = new TrackArg[depth.Length + 4];
                        args[0] = new TrackArg
                        {
                                Key = "Result"
                        };
                        depth.ForEach(a =>
                        {
                                TrackArg arg;
                                if (a == TrackDepth.接收的数据)
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Param",
                                                Value = msg.MsgBody
                                        };
                                }
                                else
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Return"
                                        };
                                }
                                args[index++] = arg;
                        });
                        args[index++] = new TrackArg
                        {
                                Key = "MsgType",
                                Value = "RpcMsg"
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "SystemType",
                                Value = RpcStateCollect.LocalSource.SystemType
                        };
                        args[index] = new TrackArg
                        {
                                Key = "LocalId",
                                Value = _LocalId
                        };
                        return args;
                }
                /// <summary>
                /// 获取发送的参数
                /// </summary>
                /// <param name="remote"></param>
                /// <param name="body"></param>
                /// <returns></returns>
                private static TrackArg[] _GetArgs(IRemote remote, string body)
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
                                                Key="LocalId",
                                                Value=_LocalId
                                        },
                                        new TrackArg
                                        {
                                                Key="SystemType",
                                                Value=RpcStateCollect.LocalSource.SystemType
                                        }
                                };
                        }
                        int index = 1;
                        TrackArg[] args = new TrackArg[depth.Length + 5];
                        args[0] = new TrackArg
                        {
                                Key = "Result"
                        };
                        depth.ForEach(a =>
                        {
                                TrackArg arg;
                                if (a == TrackDepth.发起的参数)
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Param",
                                                Value = body
                                        };
                                }
                                else
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Return"
                                        };
                                }
                                args[index++] = arg;
                        });
                        args[index++] = new TrackArg
                        {
                                Key = "MsgType",
                                Value = "RpcMsg"
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "RegionId",
                                Value = remote.RegionId.ToString()
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "LocalId",
                                Value = _LocalId
                        };
                        args[index] = new TrackArg
                        {
                                Key = "SystemType",
                                Value = RpcStateCollect.LocalSource.SystemType
                        };
                        return args;
                }
                /// <summary>
                /// 获取发送的参数
                /// </summary>
                /// <param name="remote"></param>
                /// <param name="body"></param>
                /// <returns></returns>
                private static TrackArg[] _GetArgs(IRemote remote, FileInfo file, string body)
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
                        int index = 1;
                        TrackArg[] args = new TrackArg[depth.Length + 4];
                        args[0] = new TrackArg
                        {
                                Key = "Result"
                        };
                        depth.ForEach(a =>
                        {
                                TrackArg arg;
                                if (a == TrackDepth.发起的参数)
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Param",
                                                Value = body
                                        };
                                }
                                else
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Return"
                                        };
                                }
                                args[index++] = arg;
                        });
                        args[index++] = new TrackArg
                        {
                                Key = "MsgType",
                                Value = "RpcMsg"
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "ServerId",
                                Value = remote.ServerId.ToString()
                        };
                        args[index] = new TrackArg
                        {
                                Key = "File",
                                Value = file.FullName
                        };
                        return args;
                }

                public static TrackBody CreateAnswerTrack(string type, TcpRemoteMsg msg, IPEndPoint clientIp)
                {
                        DateTime now = DateTime.Now;
                        return new TrackBody
                        {
                                Dictate = string.Concat("Answer_", type),
                                Trace = _Track.CreateAnswerTrack(msg.Track),
                                Port = clientIp.Port,
                                RemoteIp = clientIp.Address.ToString(),
                                Time = now,
                                Args = _GetAnswerArgs(msg),
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
                public static TrackBody CreateTrack(long spanId, IRemoteConfig config, IRemote remote, string body)
                {
                        DateTime now = DateTime.Now;
                        return new TrackBody
                        {
                                Dictate = string.Concat("Send_", config.SysDictate),
                                Trace = _Track.CreateTrack(spanId),
                                Port = remote.Port,
                                RemoteIp = remote.ServerIp,
                                ServerName = remote.ServerName,
                                Time = now,
                                Args = _GetArgs(remote, body),
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

                internal static bool CheckIsTrace(out long traceId)
                {
                        return _Track.CheckIsTrack(TrackRange.RpcMsg, out traceId);
                }

                public static void EndTrack(TrackBody track, TcpRemoteReply reply)
                {
                        track.Args[0].Value = "OK";
                        if (track.Args[1].Key == "Return" && reply != null)
                        {
                                track.Args[1].Value = reply.MsgBody;
                        }
                        _Track.EndTrack(track);
                }

                public static void EndTrack(TrackBody track, string error)
                {
                        track.Args[0].Value = error;
                        _Track.EndTrack(track);
                }

                public static TrackBody CreateTrack(long tranId, IRemoteConfig config, IRemote remote, FileInfo file, string body)
                {
                        DateTime now = DateTime.Now;
                        return new TrackBody
                        {
                                Dictate = string.Concat("Send_", config.SysDictate),
                                Trace = _Track.CreateTrack(tranId),
                                Port = remote.Port,
                                RemoteIp = remote.ServerIp,
                                ServerName = remote.ServerName,
                                Time = now,
                                Args = _GetArgs(remote, file, body),
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

                public static void EndTrack(TrackBody track, FileUpResult result)
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
