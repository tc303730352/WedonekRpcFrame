using System;

using RpcClient.Interface;
using RpcClient.Track;
using RpcClient.Track.Model;

using RpcHelper;

using WebSocketGateway.Interface;
namespace WebSocketGateway.Collect
{
        internal class GatwewayTrackCollect
        {
                private static readonly ITrackCollect _Track = RpcClient.RpcClient.Unity.Resolve<ITrackCollect>();
                private static readonly string _ServerId = RpcClient.RpcClient.CurrentSource.SourceServerId.ToString();
                private static TrackArg[] _GetArgs(IWebSocketService service)
                {
                        TrackDepth[] depth = _Track.GetArgTemplate();
                        if (depth.Length == 0)
                        {
                                return new TrackArg[]
                                {
                                        new TrackArg
                                        {
                                                Key="Staus"
                                        },
                                        new TrackArg
                                        {
                                                Key="MsgType",
                                                Value="WebSocket"
                                        },
                                        new TrackArg
                                        {
                                                Key="ModulerName",
                                                Value=service.ServiceName
                                        },
                                        new TrackArg
                                        {
                                                Key="AccreditId",
                                                Value=service.AccreditId
                                        },
                                        new TrackArg
                                        {
                                                Key="IdentityId",
                                                Value=service.IdentityId
                                        },
                                        new TrackArg
                                        {
                                                Key="ClientIIp",
                                                Value=service.Head.RemoteIp
                                        },
                                        new TrackArg
                                        {
                                                Key="ServerId",
                                                Value=_ServerId
                                        },
                                         new TrackArg
                                        {
                                                Key="SystemType",
                                                Value=RpcClient.RpcClient.SystemTypeVal
                                        }
                                };
                        }
                        int index = 1;
                        TrackArg[] args = new TrackArg[depth.Length + 7];
                        args[0] = new TrackArg
                        {
                                Key = "Staus"
                        };
                        depth.ForEach(a =>
                        {
                                TrackArg arg;
                                if (a == TrackDepth.发起的参数)
                                {
                                        arg = new TrackArg
                                        {
                                                Key = "Param",
                                                Value =service.PostString
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
                                Value = "WebSocket"
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "ModulerName",
                                Value = service.ServiceName
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "ServerId",
                                Value = _ServerId
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "SystemType",
                                Value = RpcClient.RpcClient.SystemTypeVal
                        };
                        args[index++] = new TrackArg
                        {
                                Key = "AccreditId",
                                Value = service.UserState?.AccreditId
                        };
                        args[index] = new TrackArg
                        {
                                Key = "ClientIIp",
                                Value = service.Head.RemoteIp
                        };
                        return args;
                }
               

                internal static bool CheckIsTrace(out long spanId)
                {
                        return _Track.CheckIsTrack(TrackRange.Gateway, out spanId);
                }
                public static TrackBody CreateTrack(IWebSocketService service, string dictate, long spanId)
                {
                        DateTime now = DateTime.Now;
                        string show = string.Empty;
                        return new TrackBody
                        {
                                Dictate = dictate,
                                Trace = _Track.CreateTrack(spanId),
                                Port = 0,
                                ServerName = show == string.Empty ? dictate : show,
                                RemoteIp = service.Head.RemoteIp,
                                Time = now,
                                Args = _GetArgs(service),
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


                public static void EndTrack(TrackBody track, IWebSocketService server)
                {
                        if (server.IsError)
                        {
                                track.Args[0].Value = server.ErrorCode;
                        }
                        else
                        {
                                track.Args[0].Value = "http.200";
                                if (track.Args[1].Key == "Return")
                                {
                                        track.Args[1].Value = server.ResponseText;
                                }
                        }
                        _Track.EndTrack(track);
                }
        }
}
