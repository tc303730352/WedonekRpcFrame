using System;
using System.IO;
using System.Net;
using WeDonekRpc.IOBuffer;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Model;
using WeDonekRpc.TcpClient.UpFile;

namespace WeDonekRpc.TcpClient
{

    public class TcpClient : ISendClient
    {
        static TcpClient ()
        {
            BufferCollect.InitBuffer();
        }

        private readonly string _ServerId = null;

        public TcpClient ()
        {
            this._ServerId = Manage.ClientManage.GetServerId(Config.SocketConfig.ServerIp, Config.SocketConfig.ServerPort);
        }
        public TcpClient (string serverId)
        {
            this._ServerId = serverId;
        }

        public TcpClient (string serverIp, int port)
        {
            this._ServerId = Manage.ClientManage.GetServerId(serverIp, port);
        }

        public static ISendClient InitClient (Uri uri, string publicKey, params string[] arg)
        {
            string str = uri.DnsSafeHost;
            IPAddress[] address = Dns.GetHostAddresses(uri.DnsSafeHost);
            if (address == null || address.Length == 0)
            {
                return null;
            }
            Config.SocketConfig.ServerIp = address[0].ToString();
            Config.SocketConfig.ServerPort = uri.Port;
            if (address.Length == 1)
            {
                Config.SocketConfig.AddConArg(Config.SocketConfig.ServerIp, uri.Port, publicKey, arg);
                return new TcpClient(address[0].ToString(), uri.Port);
            }
            else
            {
                Array.ForEach(address, a =>
                 {
                     Config.SocketConfig.AddConArg(a.ToString(), uri.Port, publicKey, arg);
                 });
                return new BatchTcpClient(address, uri.Port);
            }
        }
        public static int GetClientConNum (string serverIp, int serverPort)
        {
            string serverId = Manage.ClientManage.GetServerId(serverIp, serverPort);
            return Manage.ClientManage.GetClientConNum(serverId);
        }
        public static bool CheckIsUsable (string serverIp, int serverPort, string arg, out string error)
        {
            if (string.IsNullOrEmpty(serverIp) || serverPort <= 0)
            {
                error = "socket.server.param.error";
                return false;
            }
            if (!string.IsNullOrEmpty(arg))
            {
                Config.SocketConfig.AddConArg(serverIp, serverPort, arg);
            }
            string serverId = Manage.ClientManage.GetServerId(serverIp, serverPort);
            return Manage.ClientManage.CheckIsUsable(serverId, out error);
        }
        public bool CheckIsUsable (out string error)
        {
            if (string.IsNullOrEmpty(this._ServerId))
            {
                error = "socket.server.not.find";
                return false;
            }
            return Manage.ClientManage.CheckIsUsable(this._ServerId, out error);
        }
        public bool Ping (out TimeSpan ping, out string error)
        {
            if (string.IsNullOrEmpty(this._ServerId))
            {
                error = "socket.server.not.find";
                ping = TimeSpan.Zero;
                return false;
            }
            return Manage.ClientManage.Ping(this._ServerId, out ping, out error);
        }

        public bool Send<T, Result> (string type, T data, out Result result, out string error) where T : class where Result : class
        {
            Page page = Page.GetDataPage(this._ServerId, type, data);
            return Manage.PageManage.Send(page, out result, out error);
        }
        public bool Send<T, Result> (string type, T data, int? timeOut, out Result result, out string error) where T : class where Result : class
        {
            Page page = Page.GetDataPage(this._ServerId, type, data, timeOut);
            return Manage.PageManage.Send(page, out result, out error);
        }
        public bool Send<T> (string type, T data, out string str, out string error) where T : class
        {
            Page page = Page.GetDataPage(this._ServerId, type, data);
            return Manage.PageManage.Send(page, out str, out error);
        }
        public bool Send<Result> (string type, string data, out Result model, out string error)
        {
            if (data == null)
            {
                data = string.Empty;
            }
            Page objPage = Page.GetDataPage(this._ServerId, type, data);
            return Manage.PageManage.Send(objPage, out model, out error);
        }
        public int GetClientConNum ()
        {
            return Manage.ClientManage.GetClientConNum(this._ServerId);
        }

        public bool Send (string type, string data, out string str, out string error)
        {
            if (data == null)
            {
                data = string.Empty;
            }
            Page page = Page.GetDataPage(this._ServerId, type, data);
            return Manage.PageManage.Send(page, out str, out error);
        }


        public void Send<T> (string type, T data, Async async, object arg) where T : class
        {
            Page page = Page.GetDataPage(this._ServerId, type, data);
            Manage.PageManage.Send(page, async, arg);
        }

        public void Send (string type, string data, Async async, object arg)
        {
            Page page = Page.GetDataPage(this._ServerId, type, data);
            Manage.PageManage.Send(page, async, arg);
        }

        public bool Send<T> (string type, T data, out string error) where T : class
        {
            Page page = Page.GetSingleDataPage<T>(this._ServerId, type, data);
            return Manage.PageManage.Send(page, out error);
        }
        public bool Send<T> (string type, T data, int? timeout, out string error) where T : class
        {
            Page page = Page.GetSingleDataPage<T>(this._ServerId, type, data, timeout);
            return Manage.PageManage.Send(page, out error);
        }
        public bool Send (string type, string data, out string error)
        {
            Page page = Page.GetSingleDataPage(this._ServerId, type, data);
            return Manage.PageManage.Send(page, out error);
        }
        public void CloseClient ()
        {
            Manage.ClientManage.CloseClient(this._ServerId);
        }
        public static void CloseSocket ()
        {
            Manage.ClientManage.CloseClient();
        }

        public bool Send (string type, out string error)
        {
            Page page = Page.GetSingleDataPage(this._ServerId, type);
            return Manage.PageManage.Send(page, out error);
        }

        public bool Send (string type, out string result, out string error)
        {
            Page page = Page.GetDataPage(this._ServerId, type);
            return Manage.PageManage.Send(page, out result, out error);
        }

        public bool Send<Result> (string type, out Result result, out string error)
        {
            Page page = Page.GetDataPage(this._ServerId, type);
            return Manage.PageManage.Send(page, out result, out error);
        }
        public bool SendFile<T> (string direct, FileInfo file, T arg, UpFileAsync func, out IUpTask task, out string error)
        {
            return this.SendFile(direct, file, arg, func, null, out task, out error);
        }
        public bool SendFile<T> (string direct, FileInfo file, T arg, UpFileAsync func, UpProgressAction progress, out IUpTask upTask, out string error)
        {
            if (!file.Exists)
            {
                error = "socket.up.file.not.exists";
                upTask = null;
                return false;
            }
            UpFileTask task = new UpFileTask(direct, arg, file, func, this._ServerId, progress);
            if (!UpFileTaskCollect.GetOrAddTask(ref task))
            {
                upTask = null;
                error = task.Error;
                return false;
            }
            else
            {
                task.BeginTask();
                upTask = new UpTask(task.TaskId, this._ServerId);
                error = null;
                return true;
            }
        }
    }
}
