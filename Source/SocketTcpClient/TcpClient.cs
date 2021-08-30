using System;
using System.IO;
using System.Net;

using SocketBuffer;

using SocketTcpClient.Interface;
using SocketTcpClient.Model;
using SocketTcpClient.UpFile;

namespace SocketTcpClient
{

        public class TcpClient : ITcpClient
        {
                private readonly int _TimeOut = 0;
                private readonly int _SyncTimeOut = 0;
                static TcpClient()
                {
                        BufferCollect.InitBuffer();
                }

                private readonly string _ServerId = null;

                public int TimeOut => this._TimeOut;

                public int SyncTimeOut => this._SyncTimeOut;

                internal string ServerId => this._ServerId;

                public TcpClient()
                {
                        this._TimeOut = Config.SocketConfig.Timeout;
                        this._SyncTimeOut = Config.SocketConfig.SyncTimeOut;
                        this._ServerId = Manage.ClientManage.GetServerId(Config.SocketConfig.ServerIp, Config.SocketConfig.ServerPort);
                }
                public TcpClient(string serverId)
                {
                        this._TimeOut = Config.SocketConfig.Timeout;
                        this._SyncTimeOut = Config.SocketConfig.SyncTimeOut;
                        this._ServerId = serverId;
                }
                public TcpClient(int timeout, string serverId)
                {
                        this._TimeOut = timeout;
                        this._SyncTimeOut = timeout * 1200;
                        this._ServerId = serverId;
                }
                public TcpClient(string serverIp, int port)
                {
                        this._ServerId = Manage.ClientManage.GetServerId(serverIp, port);
                        this._TimeOut = Config.SocketConfig.Timeout;
                        this._SyncTimeOut = Config.SocketConfig.SyncTimeOut;
                }
                public TcpClient(string serverIp, int port, int timeout)
                {
                        this._TimeOut = timeout;
                        this._SyncTimeOut = timeout * 1200;
                        this._ServerId = Manage.ClientManage.GetServerId(serverIp, port);
                }
                public static ITcpClient InitClient(Uri uri, string publicKey, params string[] arg)
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
                public static int GetClientConNum(string serverIp, int serverPort)
                {
                        string serverId = Manage.ClientManage.GetServerId(serverIp, serverPort);
                        return Manage.ClientManage.GetClientConNum(serverId);
                }
                public static bool CheckIsUsable(string serverIp, int serverPort, string arg, out string error)
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
                public bool CheckIsUsable(out string error)
                {
                        if (string.IsNullOrEmpty(this._ServerId))
                        {
                                error = "socket.server.not.find";
                                return false;
                        }
                        return Manage.ClientManage.CheckIsUsable(this._ServerId, out error);
                }
                public bool Ping(out TimeSpan ping)
                {
                        if (string.IsNullOrEmpty(this._ServerId))
                        {
                                ping = TimeSpan.Zero;
                                return false;
                        }
                        return Manage.ClientManage.Ping(this._ServerId, out ping);
                }

                public bool Send<T, Result>(string type, T data, out Result result, out string error) where T : class where Result : class
                {
                        Page page = Page.GetDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out result, out error);
                }
                public bool Send<T>(string type, T data, out string str, out string error) where T : class
                {
                        Page page = Page.GetDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out str, out error);
                }
                public bool Send<Result>(string type, string data, out Result model, out string error)
                {
                        if (data == null)
                        {
                                data = string.Empty;
                        }
                        Page objPage = Page.GetDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(objPage, out model, out error);
                }
                public int GetClientConNum()
                {
                        return Manage.ClientManage.GetClientConNum(this._ServerId);
                }

                public bool Send(string type, string data, out string str, out string error)
                {
                        if (data == null)
                        {
                                data = string.Empty;
                        }
                        Page page = Page.GetDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out str, out error);
                }


                public void Send<T>(string type, T data, Async async, object arg) where T : class
                {
                        Page page = Page.GetDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        Manage.PageManage.Send(page, async, arg);
                }

                public void Send(string type, string data, Async async, object arg)
                {
                        Page page = Page.GetDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        Manage.PageManage.Send(page, async, arg);
                }

                public bool Send<T>(string type, T data, out string error) where T : class
                {
                        Page page = Page.GetSingleDataPage<T>(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out error);
                }

                public bool Send(string type, string data, out string error)
                {
                        Page page = Page.GetSingleDataPage(this._ServerId, type, data, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out error);
                }
                public void CloseClient()
                {
                        Manage.ClientManage.CloseClient(this._ServerId);
                }
                public static void CloseSocket()
                {
                        Manage.ClientManage.CloseClient();
                }

                public bool Send(string type, out string error)
                {
                        Page page = Page.GetSingleDataPage(this._ServerId, type, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out error);
                }

                public bool Send(string type, out string result, out string error)
                {
                        Page page = Page.GetDataPage(this._ServerId, type, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out result, out error);
                }

                public bool Send<Result>(string type, out Result result, out string error)
                {
                        Page page = Page.GetDataPage(this._ServerId, type, this._TimeOut, this._SyncTimeOut);
                        return Manage.PageManage.Send(page, out result, out error);
                }
                public bool SendFile<T>(string direct, FileInfo file, T arg, UpFileAsync func, out UpTask task, out string error)
                {
                        return this.SendFile(direct, file, arg, func, null, out task, out error);
                }
                public bool SendFile<T>(string direct, FileInfo file, T arg, UpFileAsync func, UpProgress progress, out UpTask upTask, out string error)
                {
                        if (!file.Exists)
                        {
                                error = "socket.up.file.not.exists";
                                upTask = null;
                                return false;
                        }
                        UpFileTask task = new UpFileTask(direct, arg, file, func, this._ServerId, progress, this._TimeOut, this._SyncTimeOut);
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
