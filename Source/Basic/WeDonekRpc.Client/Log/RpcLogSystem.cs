using System;
using System.Net;
using System.Reflection;
using Confluent.Kafka;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.FileUp;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Client.Server;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.Model;
namespace WeDonekRpc.Client.Log
{
    internal class RpcLogSystem
    {
        private const string _ConfigRefreshKey = "ConfigRefresh";
        private const string _ConFailLogKey = "RpcConFailLog";
        private const string _ConLogKey = "RpcConLog";
        private const string _MsgLogKey = "RpcMsg";
        private const string _kafkaKey = "kafka";
        private const string _CentralMsgLogKey = "CentralMsg";
        private const string _RpcLogKey = "RpcLog";
        private const string _RpcErrorLogKey = "RpcLog";
        private const string _RpcLocalErrorKey = "RpcLocalError";
        private const string _RpcQueueErrorKey = "RpcQueueError";
        private static readonly ITrackCollect _Track;
        private static readonly IRpcLogConfig _Config;

        static RpcLogSystem ()
        {
            _Track = RpcClient.Ioc.Resolve<ITrackCollect>();
            _Config = RpcClient.Ioc.Resolve<IRpcLogConfig>();
            LocalConfig.Local.RefreshEvent += Local_RefreshEvent;
        }

        private static void Local_RefreshEvent (IConfigCollect config, string name)
        {
            if (!_Config.CheckIsRecord(_ConfigRefreshKey, out LocalLogSet set))
            {
                new LogInfo("配置信息变更！", set.LogGroup, set.LogGrade)
                {
                    { "ConfigKey", name},
                    {"NewValue",config.GetValue(name) }
                }.Save();
            }
        }

        internal static void AddConFailLog (IPEndPoint ipAddress, string[] arg)
        {
            if (_Config.CheckIsRecord(_ConFailLogKey, out LocalLogSet set))
            {
                new LogInfo("已拒绝TCP链接！", set.LogGroup, set.LogGrade)
                {
                        { "arg",arg.Join("|")},
                        { "IpAddress",ipAddress }
                }.Save();
            }
        }

        internal static void AddMsgLog (string key, TcpRemoteMsg msg, TcpRemoteReply reply)
        {
            if (_Config.CheckIsRecord(_MsgLogKey, out LocalLogSet set))
            {
                new LogInfo("通讯日志！", set.LogGroup, set.LogGrade)
                {
                        { "MsgKey", key },
                        { "MsgBody", msg.MsgBody },
                        { "Source",msg.Source},
                        { "Track",msg.Track},
                        { "Tran",msg.Tran},
                        {"Retry",msg.Retry },
                        { "ExpireTime",msg.ExpireTime},
                        { "Extend",msg.Extend},
                        { "IsReply",msg.IsReply},
                        { "IsSync",msg.IsSync},
                        { "LockId",msg.LockId},
                        { "LockType",msg.LockType},
                        { "StreamLen",msg.Stream?.Length},
                        { "Result", reply?.MsgBody }
                }.Save();
            }
        }
        internal static void AddQueueError (string error, IMsg msg)
        {
            if (_Config.CheckIsRecord(_RpcQueueErrorKey, out LocalLogSet set))
            {
                new LogInfo("队列业务错误日志！", set.LogGroup, set.LogGrade)
                {
                        { "MsgKey", msg.MsgKey },
                        { "MsgBody", msg.MsgBody },
                        { "Source",msg.Source},
                        { "error",error}
                }.Save();
            }
        }
        internal static void AddConLog (Guid clientId, IPEndPoint ipAddress)
        {
            if (_Config.CheckIsRecord(_ConLogKey, out LocalLogSet set))
            {
                new LogInfo("接受了TCP链接!", set.LogGroup, set.LogGrade)
                {
                        { "ClientId",clientId },
                        { "IpAddress",ipAddress.ToString() }
                }.Save();
            }
        }

        internal static void AddKafkaErrorLog (Confluent.Kafka.Error error)
        {
            if (_Config.CheckIsRecord(_kafkaKey, LogGrade.ERROR, out LocalLogSet set))
            {
                new ErrorLog("rpc.kafka.error", "Kafka错误日志!", set.LogGroup)
                {
                        { "IsLocalError",error.IsLocalError },
                        { "ErrorCode",error.Code },
                        {"Reason",error.Reason },
                        { "IsBrokerError",error.IsBrokerError},
                        { "IsError",error.IsError},
                        { "IsFatal",error.IsFatal}
                }.Save();
            }
        }

        internal static void AddKafkaLog (LogMessage log)
        {
            LogGrade grade = _GetLogGrade(log.Level);
            if (_Config.CheckIsRecord(_kafkaKey, grade, out LocalLogSet set))
            {
                new LogInfo("Kafka消费日志", set.LogGroup, grade)
                {
                        {"Message",log.Message },
                        { "Name",log.Name},
                        { "Facility",log.Facility}
                }.Save();
            }
        }
        private static LogGrade _GetLogGrade (SyslogLevel level)
        {
            if (level == SyslogLevel.Warning)
            {
                return LogGrade.DEBUG;
            }
            else if (level == SyslogLevel.Debug)
            {
                return LogGrade.DEBUG;
            }
            else if (level == SyslogLevel.Error)
            {
                return LogGrade.ERROR;
            }
            else if (level == SyslogLevel.Info)
            {
                return LogGrade.Information;
            }
            else if (level == SyslogLevel.Notice)
            {
                return LogGrade.WARN;
            }
            else if (level == SyslogLevel.Alert)
            {
                return LogGrade.WARN;
            }
            return LogGrade.Critical;
        }

        internal static void AddKafkaErrorLog (ConsumeException e)
        {
            if (_Config.CheckIsRecord(_kafkaKey, LogGrade.ERROR, out LocalLogSet set))
            {
                ErrorException error = ErrorException.FormatError(e);
                new ErrorLog(error, "Kafka消费异常!", set.LogGroup)
                {
                        { "ErrorCode",e.Error.Code },
                        { "Reason",e.Error.Reason }
                }.Save();
            }

        }

        public static void AddErrorLog (string title, Type type, ErrorException e)
        {
            if (_Config.CheckIsRecord(_RpcErrorLogKey, e, out LocalLogSet set))
            {
                ErrorLog log = new ErrorLog(e, title, set.LogGroup)
                {
                    { "SourceType", type.FullName }
                };
                log.Save();
            }
        }
        public static void AddErrorLog (string title, Exception e)
        {
            ErrorException error = ErrorException.FormatError(e);
            if (_Config.CheckIsRecord(_RpcErrorLogKey, error, out LocalLogSet set))
            {
                new ErrorLog(error, title, set.LogGroup).Save();
            }
        }

        internal static void AddErrorLog (MethodInfo method, RemoteMsg msg, ErrorException e)
        {
            if (_Config.CheckIsRecord(_MsgLogKey, e, out LocalLogSet set))
            {
                new ErrorLog(e, "消息处理发生错误!", set.LogGroup)
                {
                        { "Method",method.ToString()},
                        { "MsgKey",msg.MsgKey},
                        { "Body",msg.MsgBody},
                        { "Source",msg.Source},
                        { "TraceId",_Track.TraceId}
                }.Save();
            }
        }


        public static void AddFileUpError (string direct, IUpFileInfo file, ErrorException e)
        {
            if (_Config.CheckIsRecord(_MsgLogKey, e, out LocalLogSet set))
            {
                new ErrorLog(e, "文件上传发生错误！", set.LogGroup)
                {
                        { "Direct",direct},
                        { "FileName",file.FileName},
                        { "Param",file.GetString()},
                        { "TraceId",_Track.TraceId}
                }.Save();
            }
        }
        public static void AddLog (string title)
        {
            if (_Config.CheckIsRecord(_RpcLogKey, out LocalLogSet set))
            {
                new LogInfo(title, set.LogGroup, set.LogGrade).Save();
            }
        }
        public static void AddLog (string title, string show)
        {
            if (_Config.CheckIsRecord(_RpcLogKey, out LocalLogSet set))
            {
                new LogInfo(title, show, set.LogGroup, set.LogGrade).Save();
            }
        }
        internal static void AddReplyErrorLog (MethodInfo method, string name, Type type, object[] param, ErrorException e)
        {
            if (_Config.CheckIsRecord(_MsgLogKey, e, out LocalLogSet set))
            {
                new ErrorLog(e, "消息处理发生错误!", set.LogGroup)
                {
                        { "Name",name},
                        { "Method",method.ToString()},
                        { "Param",param},
                        { "Source",type.FullName},
                        { "TraceId",_Track.TraceId}
                }.Save();
            }
        }
        internal static void AddReplyErrorLog (MethodInfo method, string name, object[] param, ErrorException e)
        {
            if (_Config.CheckIsRecord(_MsgLogKey, e, out LocalLogSet set))
            {
                new ErrorLog(e, "消息处理发生错误!", set.LogGroup)
                {
                        { "Name",name},
                        { "Method",method.ToString()},
                        { "Param",param},
                        { "TraceId",_Track.TraceId}
                }.Save();
            }
        }

        public static void AddMsgLog<T, Result> (string type, T data, ref Result result, RpcServer server) where T : class
        {
            if (_Config.CheckIsRecord(_CentralMsgLogKey, out LocalLogSet set))
            {
                new LogInfo("向中控服务发送消息!", set.LogGroup, set.LogGrade)
                {
                        { "Key",type},
                        { "Msg",data},
                        { "Result",result},
                        { "ServerIp",server.ServerIp},
                        { "ServerPort",server.ServerPort}
                }.Save();
            }
        }
        public static void AddMsgLog<Result> (string type, Result result, RpcServer server)
        {
            if (_Config.CheckIsRecord(_CentralMsgLogKey, out LocalLogSet set))
            {
                new LogInfo("向中控服务发送消息!", set.LogGroup, set.LogGrade)
                {
                        { "Key",type},
                        { "Result",result},
                        { "ServerIp",server.ServerIp},
                        { "ServerPort",server.ServerPort}
                }.Save();
            }
        }

        public static void AddFatalError (string title, string show, string error)
        {
            if (_Config.CheckIsRecord(_RpcLogKey, LogGrade.Critical, out LocalLogSet set))
            {
                new CriticalLog(error, title, show, set.LogGroup).Save();
            }
        }
        public static void AddLocalErrorLog<T> (string key, string title, T data, string msgType, string error) where T : class
        {
            if (_Config.CheckIsRecord(_RpcLocalErrorKey, out LocalLogSet set))
            {
                new LogInfo(title, set.LogGroup, set.LogGrade)
                {
                    {"error",error },
                    {"key",key},
                    {"Data",data},
                    {"MsgType", msgType },
                    { "TraceId", _Track.TraceId}
                }.Save(true);
            }
        }
        public static void AddMsgErrorLog<T> (string key, T data, string error, RpcServer server) where T : class
        {
            if (_Config.CheckIsRecord(_CentralMsgLogKey, out LocalLogSet set))
            {
                new LogInfo("向中控服务发送消息失败!", set.LogGroup, set.LogGrade)
                {
                        { "Key",key},
                        { "Msg",data},
                        { "error",error},
                        { "ServerIp",server.ServerIp},
                        { "ServerPort",server.ServerPort}
                }.Save();
            }
        }
        public static void AddMsgErrorLog (string key, string error, RpcServer server)
        {
            if (_Config.CheckIsRecord(_CentralMsgLogKey, out LocalLogSet set))
            {
                new LogInfo("向中控服务发送消息失败!", set.LogGroup, set.LogGrade)
                {
                        { "error",error},
                        { "Key",key},
                        { "ServerIp",server.ServerIp},
                        { "ServerPort",server.ServerPort},
                        { "TraceId",_Track.TraceId}
                }.Save();
            }
        }
        internal static void AddMsgErrorLog (RemoteMsg msg, TcpRemoteReply reply, string error)
        {
            if (_Config.CheckIsRecord(_MsgLogKey, out LocalLogSet set))
            {
                new LogInfo("向其它节点回复信息失败", set.LogGroup, set.LogGrade)
                {
                    {"error",error },
                    {"Key",msg.MsgKey },
                    {"Msg",msg.MsgBody },
                    {"MsgType","MsgReply" },
                    {"ReplyMsg", reply.MsgBody },
                    {"TraceId",_Track.TraceId }
                }.Save();
            }
        }
    }
}
