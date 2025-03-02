using System;
using System.Text;
using WeDonekRpc.ApiGateway;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Track;
using WeDonekRpc.Client.Track.Model;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Interface;
namespace WeDonekRpc.HttpApiGateway.Collect
{
    internal class GatwewayTrackCollect
    {
        private static readonly ITrackCollect _Track = RpcClient.Ioc.Resolve<ITrackCollect>();
        private static readonly string _ServerId = RpcClient.CurrentSource.ServerId.ToString();
        private static TrackArg[] _GetArgs (IService service)
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
                            Value="Gaweway"
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
                            Key="ClientIIp",
                            Value=service.Request.ClientIp
                    },
                    new TrackArg
                    {
                            Key="LocalId",
                            Value=_ServerId
                    },
                    new TrackArg
                    {
                            Key="SystemType",
                            Value=RpcClient.SystemTypeVal
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
                        Value = _GetParamStr(service)
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
                Value = "Gaweway"
            };
            args[index++] = new TrackArg
            {
                Key = "ModulerName",
                Value = service.ServiceName
            };
            args[index++] = new TrackArg
            {
                Key = "LocalId",
                Value = _ServerId
            };
            args[index++] = new TrackArg
            {
                Key = "SystemType",
                Value = RpcClient.SystemTypeVal
            };
            args[index++] = new TrackArg
            {
                Key = "AccreditId",
                Value = service.AccreditId
            };
            args[index] = new TrackArg
            {
                Key = "ClientIIp",
                Value = service.Request.ClientIp
            };
            return args;
        }
        private static string _GetParamStr (IService service)
        {
            if (service.Request.HttpMethod == "GET")
            {
                return service.Request.Url.Query;
            }
            else if (service.Request.IsPostFile)
            {
                StringBuilder str = new StringBuilder();
                _ = str.Append("Files:\r\n");
                service.Request.Files.ForEach(a =>
                {
                    _ = str.AppendFormat("name={0},size={1},type:{2}\r\n", a.FileName, a.FileSize, a.FileType);
                });
                if (!service.Request.PostString.IsNull())
                {
                    _ = str.Append("post: ");
                    _ = str.Append(service.Request.PostString);
                }
                return str.ToString();
            }
            else if (!service.Request.PostString.IsNull())
            {
                return service.Request.PostString;
            }
            else
            {
                return string.Empty;
            }
        }

        internal static bool CheckIsTrace (IService service, out long spanId)
        {
            if (_Track.CheckIsTrack(TrackRange.Gateway, out spanId))
            {
                return true;
            }
            else if (service.Request.Headers["IsTrace"] == "true" && _Track.Config.IsEnable)
            {
                string pwd = service.Request.Headers["TracePwd"];
                if (pwd.IsNull() || pwd != GatewayServer.Config.TracePwd)
                {
                    return false;
                }
                spanId = _Track.ApplySpanId();
                return true;
            }
            return false;
        }
        public static TrackBody CreateTrack (IService service, long spanId)
        {
            DateTime now = DateTime.Now;
            Uri uri = service.Request.Url;
            string show = ApiGatewayService.GetApiShow(service.Request.Url);
            return new TrackBody
            {
                Dictate = service.Request.Url.GetLeftPart(UriPartial.Path),
                StageType = StageType.Send,
                Trace = _Track.CreateTrack(spanId),
                Port = uri.Port,
                Show = show,
                RemoteIp = uri.Authority,
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


        public static void EndTrack (TrackBody track, IService server)
        {
            if (server.IsError)
            {
                track.Args[0].Value = server.LastError;
            }
            else
            {
                track.Args[0].Value = "http.200";
                if (track.Args[1].Key == "Return")
                {
                    track.Args[1].Value = server.Response.ResponseTxt;
                }
            }
            _Track.EndTrack(track);
        }
    }
}
