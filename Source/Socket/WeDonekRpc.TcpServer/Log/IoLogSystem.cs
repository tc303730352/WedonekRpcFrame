using System;
using System.Net.Sockets;
using System.Text;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.IOBuffer.Interface;
using WeDonekRpc.TcpServer.Client;
using WeDonekRpc.TcpServer.Config;
using WeDonekRpc.TcpServer.Interface;
using WeDonekRpc.TcpServer.Model;
namespace WeDonekRpc.TcpServer.Log
{
    internal class IoLogSystem
    {
        private static readonly string _LogGroup = "TcpServerLog";
        private static readonly string _SendSocketGroup = "SendSocketException";
        private static readonly string _SendGroup = "SendException";

        private static readonly string _ReceiveSocketGroup = "ReceiveSocketException";
        private static readonly string _ReceiveGroup = "ReceiveException";


        private static readonly string _InitSocketGroup = "InitSocketException";
        private static readonly string _InitGroup = "InitException";

        private static readonly string _ListenSocketGroup = "ListenSocketException";
        private static readonly string _ListenGroup = "ListenException";


        private static readonly string _ReplyPageError = "ReplyPageError";

        private static readonly string _PageError = "PageError";

        private static readonly string _ConCloseError = "ConClose";
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
                    {"content",str.ToString() },
                    {"page",data?.ToJson() },
                    { "RemoteIp", client.RemoteIp?.ToString() },
                    { "Port", client.Port.ToString() }
                };
                log.Save();
            }
        }

        public static void AddConClose(SocketClient client, Socket socket, int time)
        {
            if (SocketLogConfig.CheckIsRecord(_ConCloseError, out LogGrade grade))
            {
                LogInfo log = new LogInfo("客户端链接断开事件!", string.Empty, _LogGroup, grade)
                {
                    { "RemoteIp", client.RemoteIp.ToString() },
                    { "Port", client.Port.ToString() },
                    { "ColseTime", time.ToString() },
                    {"Connected", socket.Connected.ToString()},
                    { "BindParam", client.Client.BindParam },
                    { "IsAuthorization", client.Client.IsAuthorization.ToString() },
                };
                log.Save();
            }
        }
        public static void AddConClose(SocketClient client, Socket socket)
        {
            if (SocketLogConfig.CheckIsRecord(_ConCloseError, out LogGrade grade))
            {
                LogInfo log = new LogInfo("客户端链接断开事件!", string.Empty, _LogGroup, grade)
                {
                    { "RemoteIp", client.RemoteIp.ToString() },
                    { "Port", client.Port.ToString() },
                    {"Connected", socket.Connected.ToString()},
                    { "BindParam", client.Client.BindParam },
                    { "IsAuthorization", client.Client.IsAuthorization.ToString() },
                };
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
                error.Args.Add("RemoteIp", socket.RemoteIp.ToString());
                error.Args.Add("Port", socket.Port.ToString());
                error.Args.Add("IsAuthorization", socket.Client.IsAuthorization.ToString());
                error.Args.Add("BindParam", socket.Client.BindParam);
                error.Save(_LogGroup);
            }
        }
        public static void AddSendLog(Exception e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_SendGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                error.Args.Add("Port", client.Port.ToString());
                error.Args.Add("IsAuthorization", client.Client.IsAuthorization.ToString());
                error.Args.Add("BindParam", client.Client.BindParam);
                error.Save(_LogGroup);
            }
        }
        public static void AddReceiveLog(Exception e, SocketClient client)
        {
            if (SocketLogConfig.CheckIsRecord(_ReceiveGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("RemoteIp", client.RemoteIp.ToString());
                error.Args.Add("Port", client.Port.ToString());
                error.Args.Add("IsAuthorization", client.Client.IsAuthorization.ToString());
                error.Args.Add("BindParam", client.Client.BindParam);
                error.Save(_LogGroup);
            }
        }
        public static void AddReceiveLog(SocketException e, SocketClient socket)
        {
            if (SocketLogConfig.CheckIsRecord(_ReceiveSocketGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                error.Args.Add("RemoteIp", socket.RemoteIp.ToString());
                error.Args.Add("Port", socket.Port.ToString());
                error.Args.Add("IsAuthorization", socket.Client.IsAuthorization.ToString());
                error.Args.Add("BindParam", socket.Client.BindParam);
                error.Save(_LogGroup);
            }
        }


        internal static void AddInitLog(SocketException e, SocketClient socket)
        {
            if (SocketLogConfig.CheckIsRecord(_InitSocketGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                if (socket.RemoteIp != null)
                {
                    error.Args.Add("RemoteIp", socket.RemoteIp.ToString());
                }
                error.Args.Add("Port", socket.Port.ToString());
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
                error.Args.Add("Port", client.Port.ToString());
                error.Save(_LogGroup);
            }
        }
        internal static void AddListenLog(SocketException e)
        {
            if (SocketLogConfig.CheckIsRecord(_ListenSocketGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Args.Add("SocketErrorCode", e.SocketErrorCode.ToString());
                error.Args.Add("ErrorCode", e.ErrorCode.ToString());
                error.Args.Add("NativeErrorCode", e.NativeErrorCode.ToString());
                error.Save(_LogGroup);
            }
        }
        internal static void AddListenLog(Exception e)
        {
            if (SocketLogConfig.CheckIsRecord(_ListenGroup, out LogGrade grade))
            {
                ErrorException error = ErrorException.FormatError(e, grade);
                error.Save(_LogGroup);
            }
        }

        internal static void AddReplyError(ReplyPage reply, string error)
        {
            if (SocketLogConfig.CheckIsRecord(_ReplyPageError, out LogGrade grade))
            {
                Page page = reply.page;
                LogInfo log = new LogInfo(error, grade, _LogGroup)
                {
                    { "PageId",page.PageId.ToString()},
                    { "Type",page.Type},
                    {"BindParam",reply.client.BindParam },
                    { "RempteIp",reply.client.RempteIp.ToString()}
                 };
                log.LogTitle = "数据包回复失败!";
                log.Save();
            }
        }
        internal static void AddReplyError(Page page, IClient client, string error)
        {
            if (SocketLogConfig.CheckIsRecord(_ReplyPageError, out LogGrade grade))
            {
                LogInfo log = new LogInfo(error, grade, _LogGroup)
                {
                    { "PageId",page.PageId.ToString()},
                    { "Type",page.Type},
                    {"BindParam",client.BindParam },
                    { "RempteIp",client.RempteIp?.ToString()}
                 };
                log.LogTitle = "数据包回复失败!";
                log.Save();
            }
        }
    }
}
