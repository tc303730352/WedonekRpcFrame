using System;

using SocketTcpServer.Interface;
using SocketTcpServer.Manage;
using SocketTcpServer.Model;
using SocketTcpServer.Server;

namespace SocketTcpServer
{
        public class SocketClient
        {
                private Guid _ServerId = Guid.Empty;
                private readonly int _TimeOut = 0;
                private readonly int _SyncTimeOut = 0;
                public SocketClient(Guid serverId)
                {
                        this._ServerId = serverId;
                        this._TimeOut = Config.SocketConfig.TimeOut;
                        this._SyncTimeOut = Config.SocketConfig.SyncTimeOut;
                }
                public SocketClient(Guid serverId, int timeOut)
                {
                        this._ServerId = serverId;
                        this._TimeOut = timeOut;
                        this._SyncTimeOut = timeOut * 1200;
                }
                public SocketClient()
                {
                        this._ServerId = SocketTcpServer.DefaultServerId;
                        this._TimeOut = Config.SocketConfig.TimeOut;
                        this._SyncTimeOut = Config.SocketConfig.SyncTimeOut;
                }
                public SocketClient(int port, int timeOut)
                {
                        this._ServerId = SocketTcpServer.GetServerId(port);
                        this._TimeOut = timeOut;
                        this._SyncTimeOut = timeOut * 1200;
                }
                public bool CheckState(Guid clientId)
                {
                        return SocketTcpServer.GetServer(this._ServerId, out ServerInfo server) && server.CheckIsCon(clientId);
                }
                public SocketClient(int port)
                {
                        this._ServerId = SocketTcpServer.GetServerId(port);
                        this._TimeOut = Config.SocketConfig.TimeOut;
                        this._SyncTimeOut = Config.SocketConfig.SyncTimeOut;
                }

                public void BatchSend<T>(string type, T data, Guid[] clientId)
                {
                        SocketTcpServer.BatchSend(this._ServerId, type, data, clientId, this._TimeOut, this._SyncTimeOut);
                }

                public bool Send<T>(string type, Guid clientId, T data, out string error) where T : class
                {
                        return PageManage.Send(Page.GetSingleDataPage(this._ServerId, clientId, type, data, this._TimeOut, this._SyncTimeOut), out error);
                }
                public void Send<T>(string type, Guid clientId, T data, Async async, object arg) where T : class
                {
                        PageManage.Send(Page.GetSingleDataPage(this._ServerId, clientId, type, data, this._TimeOut, this._SyncTimeOut), async, arg);
                }
                public void Send(string type, Guid clientId, Async async, object arg)
                {
                        PageManage.Send(Page.GetSingleDataPage(this._ServerId, clientId, type, this._TimeOut, this._SyncTimeOut), async, arg);
                }
                public bool Send<T, Result>(string type, Guid clientId, T data, out Result result, out string error) where T : class where Result : class
                {
                        return PageManage.Send(Page.GetDataPage(this._ServerId, clientId, type, data, this._TimeOut, this._SyncTimeOut), out result, out error);
                }
                public bool Send<Result>(string type, Guid clientId, out Result result, out string error) where Result : class
                {
                        return PageManage.Send(Page.GetDataPage(this._ServerId, clientId, type, this._TimeOut, this._SyncTimeOut), out result, out error);
                }
        }
}
