using System;
using System.Net;
using System.Net.Sockets;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Collect;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket.Server
{
    internal class SocketServer : ISocketServer
    {
        private readonly Socket _Socket = null;

        /// <summary>
        /// 客户端列表集合
        /// </summary>
        private readonly ClientCollect _ClientList = null;

        private readonly IClientSessionCollect _Session = null;


        private readonly ISocketEvent _Event;

        public SocketServer (IWebSocketConfig config)
        {
            this.Config = config;
            this._Event = config.SocketEvent;
            this._ClientList = new ClientCollect(this);
            this._Session = new ClientSessionCollect(this);
            this.IsSSL = config.Certificate != null;
            this._Socket = this._InitSocket();
        }
        public IClientSessionCollect Session => this._Session;
        public IWebSocketConfig Config
        {
            get;
        }

        public bool IsSSL { get; }

        public Guid Id { get; } = Guid.NewGuid();

        private Socket _InitSocket ()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, this.Config.ServerPort));
            socket.Listen(this.Config.TcpBacklog);
            _ = socket.BeginAccept(this._ClientAccept, null);
            return socket;
        }
        public void Sync ()
        {
            this._Session.ClearSession();
        }

        public bool Authorize (RequestBody request)
        {
            try
            {
                return this._Event.Authorize(request);
            }
            catch (Exception e)
            {
                LogSystem.AddErrorLog(e, "Authorize");
                return false;
            }
        }

        public void CheckSessionState (IWebSocketClient client)
        {
            if (!client.IsAuthorize || !client.Session.IsAccredit)
            {
                return;
            }
            try
            {
                this._Event.CheckSession(client.Session);
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                if (error.IsSystemError)
                {
                    LogSystem.AddErrorLog(error, client.Session);
                    return;
                }
                this._ReplyError(new ApiService(client), error, "SessionEnd");
                client.CloseClient(20);
            }
        }

        public void ReceiveData (IWebSocketClient client, PageType type, byte[] content)
        {
            if (type == PageType.pong)
            {
                return;
            }
            else if (type == PageType.close)
            {
                client.CloseClient();
            }
            else if (type == PageType.ping)
            {
                if (!content.IsNull())
                {
                    this._ReceiveUserPage(client, content);
                }
                _ = client.SendPong();
            }
            else
            {
                this._ReceiveUserPage(client, content);
            }
        }

        private void _ReceiveUserPage (IWebSocketClient client, byte[] content)
        {
            IApiService service = new ApiService(client, content);
            try
            {
                this.Config.SocketEvent.Receive(service);
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                this._ReplyError(service, error, "ReceivePage");
            }
        }
        public void CloseCon (Guid clientId)
        {
            if (this._ClientList.GetClient(clientId, out IWebSocketClient client))
            {
                client.CloseClient();
            }
        }

        private void _ClientAccept (IAsyncResult ar)
        {
            Socket client = this._Socket.EndAccept(ar);
            _ = this._Socket.BeginAccept(this._ClientAccept, null);
            this._ClientList.AddClient(client);
        }



        public bool CheckIsOnline (Guid clientId)
        {
            if (this._ClientList.GetClient(clientId, out IWebSocketClient client))
            {
                return client.IsCon;
            }
            return false;
        }

        public void RemoveClient (Guid clientId)
        {
            if (this._ClientList.RemoveClient(clientId, out IWebSocketClient client) && client.IsAuthorize)
            {
                ISession session = this._Session.Offline(client.Session.SessionId);
                if (session != null)
                {
                    this._Event.SessionOffline(session);
                }
            }
        }

        public void CloseCon (Guid clientId, int time)
        {
            if (this._ClientList.GetClient(clientId, out IWebSocketClient client))
            {
                client.CloseClient(time);
            }
        }


        public void AuthorizeComplate (IWebSocketClient client)
        {
            IApiService service = new ApiService(client);
            try
            {
                this._Event.AuthorizeComplate(service);
                client.Session.Accredit();
            }
            catch (Exception e)
            {
                ErrorException error = ErrorException.FormatError(e);
                this._ReplyError(service, error, "AuthorizeComplate");
            }
        }
        public void ReplyError (IWebSocketClient client, ErrorException error, string source)
        {
            this._ReplyError(new ApiService(client), error, source);
        }
        private void _ReplyError (IApiService service, ErrorException error, string source)
        {
            LogSystem.AddErrorLog(error, service, source);
            this._Event.ReplyError(service, error, source);
        }

        public void Close ()
        {
            this._Socket.Close();
        }
    }
}
