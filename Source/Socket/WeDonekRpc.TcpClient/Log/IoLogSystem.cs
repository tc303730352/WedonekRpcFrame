using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.IOBuffer.Interface;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Client;
using WeDonekRpc.TcpClient.Config;
using WeDonekRpc.TcpClient.Interface;
using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.Log
{
    internal class IoLogSystem
    {
        private static readonly string _LogGroup = "TcpClientLog";
        private static readonly string _SendSocketGroup = "SendSocketException";
        private static readonly string _SendGroup = "SendException";

        private static readonly string _ReceiveSocketGroup = "ReceiveSocketException";
        private static readonly string _ReceiveGroup = "ReceiveException";

        private static readonly string _InitSocketGroup = "InitSocketException";
        private static readonly string _InitGroup = "InitException";
        private static readonly string _ConnectGroup = "ConnectException";

        private static readonly string _LeaveUnused = "LeaveUnsedEvent";
        private static readonly string _ConCloseError = "ConClose";
        public static void AddConClose(SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_ConCloseError, out LogGrade grade))
            {
                LogInfo log = new LogInfo("链接断开事件!", string.Empty, _LogGroup, grade);
                if (client.LocalPoint != null)
                {
                    log.Add("local", client.LocalPoint.ToString());
                }
                if (client.RemoteIp != null)
                {
                    log.Add("RemoteIp", client.RemoteIp.ToString());
                }
                log.Save();
            }
        }
        public static void AddConClose(SocketClient client, int time)
        {
            if (SocketLogConfig.CheckIsRecord(_ConCloseError, out LogGrade grade))
            {
                LogInfo log = new LogInfo("链接断开事件!", string.Empty, _LogGroup, grade)
                {
                    { "Time", time.ToString() },
                    {"Connected", client.Connected.ToString()}
                };
                if (client.LocalPoint != null)
                {
                    log.Add("local", client.LocalPoint.ToString());
                }
                if (client.RemoteIp != null)
                {
                    log.Add("RemoteIp", client.RemoteIp.ToString());
                }
                log.Save();
            }
        }
        public static void AddSendLog(SocketException e, SocketClient socket)
        {
            if (SocketLogConfig.CheckIsRecord(_SendSocketGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                if (socket.LocalPoint != null)
                {
                    error.Args.Add("local", socket.LocalPoint.ToString());
                }
                if (socket.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", socket.RemoteIp.ToString());
                }
                error.Save(_LogGroup);
            }
        }
        public static void AddSendLog(Exception e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_SendGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                if (client.LocalPoint != null)
                {
                    error.Args.Add("local", client.LocalPoint.ToString());
                }
                if (client.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                }
                error.Save(_LogGroup);
            }
        }
        public static void AddReceiveLog(Exception e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_ReceiveGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                if (client.LocalPoint != null)
                {
                    error.Args.Add("local", client.LocalPoint.ToString());
                }
                if (client.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                }
                error.Save(_LogGroup);
            }
        }
        public static void AddReceiveLog(SocketException e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_ReceiveSocketGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                if (client.LocalPoint != null)
                {
                    error.Args.Add("local", client.LocalPoint.ToString());
                }
                if (client.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                }
                error.Save(_LogGroup);
            }
        }

        internal static void AddInitLog(SocketException e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_InitSocketGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                if (client.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                }
                if (client.LocalPoint != null)
                {
                    error.Args.Add("local", client.LocalPoint.ToString());
                }
                error.Save(_LogGroup);
            }
        }
        internal static void AddInitLog(Exception e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_InitGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                if (client.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                }
                if (client.LocalPoint != null)
                {
                    error.Args.Add("local", client.LocalPoint.ToString());
                }
                error.Save(_LogGroup);
            }
        }

        internal static void AddConnectLog(SocketException e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_ConnectGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                if (client.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                }
                if (client.LocalPoint != null)
                {
                    error.Args.Add("local", client.LocalPoint.ToString());
                }
                error.Save(_LogGroup);
            }
        }

        internal static void AddLeaveUnusedLog(IClient a, int time)
        {
            if (SocketLogConfig.CheckIsRecord(_LeaveUnused, out LogGrade grade))
            {
                LogInfo log = new LogInfo("链接闲置释放事件!", "闲置时间超过：" + Tools.FormatTime(time - a.LastTime), _LogGroup, grade)
                {
                    { "RemoteIp", a.ServerId }
                };
                if (a.Client.LocalPoint != null)
                {
                    log.Add("Local", a.Client.LocalPoint.ToString());
                }
                log.Save();
            }
        }

        internal static void AddErrorLog(Exception e, string title)
        {
            if (LogSystem.CheckIsRecord(LogGrade.Information))
            {
                new LogInfo(e, _LogGroup, LogGrade.Information)
                {
                    LogTitle = title
                }.Save();
            }
        }

        internal static void AddTimeOutPage(string type, byte[] content, byte dataType)
        {
            if (LogSystem.CheckIsRecord(LogGrade.WARN))
            {
                new LogInfo(type + ",回复超时!", _LogGroup, LogGrade.WARN)
                {
                    {"Instruct",type },
                    {"Content",Encoding.UTF8.GetString(content) },
                    {"DataType",((SendType)dataType).ToString() }
                }.Save();
            }
        }

        internal static void AddErrorPageLog(ISocketBuffer buffer, SocketClient client, DataPageInfo data)
        {
            if (LogSystem.CheckIsRecord(LogGrade.WARN))
            {
                StringBuilder str = new StringBuilder();
                buffer.BufferSize.For(c =>
                {
                    _ = str.Append('|');
                    _ = str.Append(buffer.Stream[c].ToString());
                });
                _ = str.Remove(0, 1);
                LogInfo log = new LogInfo("收到错误包!", _LogGroup, LogGrade.WARN)
                {
                    {"size",buffer.BufferSize.ToString() },
                    {"ver",buffer.Ver },
                    {"curThreadId",Thread.CurrentThread.ManagedThreadId },
                    {"content",str.ToString() },
                    {"page",data?.ToJson() }
                };
                if (client.RemoteIp != null)
                {
                    log.Add("RemoteIp", client.RemoteIp.ToString());
                }
                if (client.LocalPoint != null)
                {
                    log.Add("local", client.LocalPoint.ToString());
                }
                log.Save();
            }
        }
    }
}
