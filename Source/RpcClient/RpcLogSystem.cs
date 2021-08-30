using System;
using System.Net;
using System.Reflection;

using RpcClient.Config;
using RpcClient.Interface;
using RpcClient.Model;
using RpcClient.Server;

using RpcHelper;

using RpcModel;
namespace RpcClient
{
        internal class RpcLogSystem
        {
                private static readonly ITrackCollect _Track;
                private static readonly string _DefGroup = "Rpc";
                private static readonly string _RpcMsgGroup = "Rpc_Msg";

                static RpcLogSystem()
                {
                        _Track = RpcClient.Unity.Resolve<ITrackCollect>();
                }
                internal static void AddConLog(IPEndPoint ipAddress, string error)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("拒绝其它服务节点TCP链接！", _DefGroup)
                                {
                                     { "Reason",error},
                                     { "IpAddress",ipAddress }
                                }.Save();
                        }
                }

                internal static void AddMsgLog(string key, TcpRemoteMsg msg, TcpRemoteReply reply)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("通讯日志！", _RpcMsgGroup)
                                {
                                        { "MsgKey", key },
                                        { "Msg", msg },
                                        { "Result", reply?.MsgBody }
                                }.Save();
                        }
                }

                internal static void AddConLog(Guid clientId, IPEndPoint ipAddress)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("接受了其它服务节点链接!", _DefGroup)
                                {
                                        { "ClientId",clientId },
                                        { "IpAddress",ipAddress }
                                }.Save();
                        }
                }

                public static void AddErrorLog(string title, Type type, ErrorException e)
                {
                        if (LogSystem.CheckIsRecord(e))
                        {
                                new ErrorLog(e, title, _DefGroup)
                                {
                                        LogContent = string.Concat("Source:", type.FullName)
                                }.Save();
                        }
                }


                internal static void AddErrorLog(MethodInfo method, RemoteMsg msg, ErrorException e)
                {
                        if (LogSystem.CheckIsRecord(e))
                        {
                                new ErrorLog(e, "调用消息委托发生错误!", _RpcMsgGroup)
                                {
                                        { "Method",method.ToString()},
                                        { "MsgKey",msg.MsgKey},
                                        { "Body",msg.MsgBody},
                                        { "Source",msg.Source},
                                        { "TraceId",_Track.TraceId}
                                }.Save();
                        }
                }
             

                public static void AddFileUpError<T>(string direct, UpFileDatum<T> file, ErrorException e) where T : class
                {
                        if (LogSystem.CheckIsRecord(e))
                        {
                                new ErrorLog(e, "文件上传发生错误！", _DefGroup)
                                {
                                        { "Direct",direct},
                                        { "FileName",file.FileName},
                                        { "Source",file.Source},
                                        { "Param",file.Param},
                                        { "TraceId",_Track.TraceId}
                                }.Save();
                        }
                }
                public static void AddLog(string title)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.Information))
                        {
                                new InfoLog(title, _DefGroup).Save();
                        }
                }
                public static void AddRouteLog(MethodInfo method, string name, Type source, string error)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.ERROR))
                        {
                                new ErrorLog(new ErrorException(error), "路由注册失败!", _DefGroup)
                                {
                                        { "Name",name},
                                        { "Method",method.ToString()},
                                        { "Source",source.FullName}
                                }.Save();
                        }
                }
                public static void AddRouteLog(MethodInfo method, string name, Type source)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("路由注册", _DefGroup)
                                {
                                        { "Name",name},
                                        { "Method",method.ToString()},
                                        { "Source",source.FullName}
                                }.Save();
                        }
                }



                internal static void AddRouteErrorLog(MethodInfo method, string name, Type type, object[] param, ErrorException e)
                {
                        if (LogSystem.CheckIsRecord(e))
                        {
                                new ErrorLog(e, "消息处理发生错误!", _DefGroup)
                                {
                                        { "Name",name},
                                        { "Method",method.ToString()},
                                        { "Param",param},
                                        { "Source",type.FullName},
                                        { "TraceId",_Track.TraceId}
                                }.Save();
                        }
                }
                internal static void AddRouteErrorLog(MethodInfo method, string name, object[] param, ErrorException e)
                {
                        if (LogSystem.CheckIsRecord(e))
                        {
                                new ErrorLog(e, "消息处理发生错误!", _DefGroup)
                                {
                                        { "Name",name},
                                        { "Method",method.ToString()},
                                        { "Param",param},
                                        { "TraceId",_Track.TraceId}
                                }.Save();
                        }
                }

                public static void AddMsgLog<T, Result>(string type, T data, Result result, RpcServer server) where T : class
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("向中心服务发送消息!", _RpcMsgGroup)
                                {
                                        { "Key",type},
                                        { "Msg",data},
                                        { "Result",result},
                                       { "ServerIp",server.ServerIp},
                                        { "ServerPort",server.ServerPort}
                                }.Save();
                        }
                }
                public static void AddMsgLog<Result>(string type, Result result, RpcServer server)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("向中心服务发送消息!", _RpcMsgGroup)
                                {
                                        { "Key",type},
                                        { "Result",result},
                                         { "ServerIp",server.ServerIp},
                                        { "ServerPort",server.ServerPort}
                                }.Save();
                        }
                }

                public static void AddFatalError(string title, string show, string error)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.Critical))
                        {
                                new CriticalLog(error, title, show, _DefGroup).Save();
                        }
                }
                public static void AddRemoteLog(IRemote remote, int oldState)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.Information))
                        {
                                LogInfo log = new InfoLog("远程服务节点状态变更!", null, _DefGroup)
                                {
                                        { "TraceId",_Track.TraceId}
                                };
                                log.AddOrSet(remote.ToDictionary(oldState));
                                log.Save();
                        }
                }
                public static void AddLocalErrorLog<T>(string key, T data, string msgType, string error) where T : class
                {
                        if (LogSystem.CheckIsRecord(LogGrade.WARN))
                        {
                                new WarnLog(error, "错误消息发送失败!", _RpcMsgGroup)
                                {
                                        { "Key",key},
                                        { "Msg",data},
                                        { "MsgType",msgType},
                                        { "TraceId",_Track.TraceId}
                                }.Save(true);
                        }
                }
                public static void AddMsgErrorLog<T>(string key, T data, string error, RpcServer server) where T : class
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("向中心服务发送消息失败!", _DefGroup)
                                {
                                        { "Key",key},
                                        { "Msg",data},
                                        { "error",error},
                                        { "ServerIp",server.ServerIp},
                                        { "ServerPort",server.ServerPort}
                                }.Save();
                        }
                }
                public static void AddMsgErrorLog(string key, string error, RpcServer server)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.DEBUG))
                        {
                                new DebugLog("向中心服务发送消息失败!", _DefGroup)
                                {
                                        { "error",error},
                                        { "Key",key},
                                        { "ServerIp",server.ServerIp},
                                        { "ServerPort",server.ServerPort},
                                        { "TraceId",_Track.TraceId}
                                }.Save();
                        }
                }
                internal static void AddMsgErrorLog(RemoteMsg msg, TcpRemoteReply reply, string error)
                {
                        if (LogSystem.CheckIsRecord(LogGrade.WARN))
                        {
                                new WarnLog(error, "向其它节点回复信息失败!", _RpcMsgGroup)
                                {
                                        { "Key",msg.MsgKey},
                                        { "Msg",msg.MsgBody},
                                        { "MsgType","MsgReply"},
                                        { "ReplyMsg",reply.MsgBody},
                                        { "TraceId",_Track.TraceId}
                                }.Save();
                        }
                }
        }
}
