using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Config;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket.Client
{

    internal class WebSocketClient : IWebSocketClient
    {

        private ISession _Session = null;


        private readonly lSocketClient _Client = null;

        private readonly ISocketServer _Server = null;

        private volatile int _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;

        public Guid ClientId
        {
            get;
        } = Guid.NewGuid();
        public int HeartbeatTime => this._HeartbeatTime;

        public bool IsCon => this._Client.IsCon;
        public bool IsAuthorize { get; private set; }

        public RequestBody Request { get; private set; }

        public ISession Session => this._Session;


        public WebSocketClient (Socket socket, ISocketServer server)
        {
            this._Server = server;
            this._Client = server.IsSSL ? new SslSocketClient(socket, this, server) : new SocketClient(socket, this);
        }
        public void InitSocket ()
        {
            this._Client.InitSocket();
        }
        /// <summary>
        /// 检查授权
        /// </summary>
        /// <param name="head"></param>
        /// <param name="remoteIp"></param>
        /// <returns></returns>
        public bool CheckAuthorize (HttpHead head, IPEndPoint remoteIp)
        {
            this.Request = new RequestBody(head, remoteIp.Address.ToString());
            if (!this._Server.Authorize(this.Request))
            {
                return false;
            }
            string key = Tools.Sha1ToBase64(string.Concat(head.WebSocketKey, this._Server.Config.ResponseGuid), Encoding.ASCII);
            string replyStr = string.Format("HTTP/1.1 101 Switching Protocols\r\nUpgrade: websocket\r\nConnection: Upgrade\r\nSec-WebSocket-Accept: {0}\r\n\r\n", key);
            if (!this._Client.Send(Encoding.ASCII.GetBytes(replyStr)))
            {
                return false;
            }
            this._Session = this._Server.Session.CreateSession(this);
            return this._Session != null;
        }
        public void AuthorizeComplate ()
        {
            this._Server.AuthorizeComplate(this);
            this._Client.Authorize();
            this.IsAuthorize = true;
        }

        public void ConCloseEvent ()
        {
            this._Server.RemoveClient(this.ClientId);
        }

        public async void Receive (DataPageInfo page)
        {
            this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            if (this.Session.IsAccredit)
            {
                _ = ThreadPool.UnsafeQueueUserWorkItem(this._Receive, page, true);
            }
        }
        private void _Receive (DataPageInfo page)
        {
            this._Server.ReceiveData(this, page.PageType, page.Content);
        }

        public void Dispose ()
        {
            this._Client.Dispose();
        }
        private bool _Check ()
        {
            return this.IsCon;
        }

        private bool _Send (byte[] datas, PageType type)
        {
            if (!this._Client.Send(SocketTools.GetResponsePage(datas, type)))
            {
                return false;
            }
            this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            return true;
        }
        public bool SendPong ()
        {
            return this._Client.Send(SocketTools.Pong);
        }
        private bool _SendClosePage ()
        {
            return this._Client.Send(SocketTools.Close);
        }
        public bool CheckClient ()
        {
            if (this._Client.IsAuthorize)
            {
                return this._Client.Send(SocketTools.Ping);
            }
            else
            {
                this._Client.Close();
                return false;
            }
        }
        public void CloseClient (int time)
        {
            if (this._Client.IsAuthorize)
            {
                if (this._Client.Close(time))
                {
                    _ = this._SendClosePage();
                }
            }
            else
            {
                this._Client.Close();
            }
        }
        public void CloseClient ()
        {
            this.CloseClient(10);
        }

        public bool Send (string text)
        {
            if (this._Check())
            {
                byte[] datas = PublicConfig.ResponseEncoding.GetBytes(text);
                return this._Send(datas, PageType.Text);
            }
            return false;
        }

        public bool Send (Stream stream)
        {
            if (!this._Check())
            {
                return false;
            }
            using (stream)
            {
                stream.Position = 0;
                int size = this._Server.Config.BufferSize;
                if (stream.Length <= size)
                {
                    if (this._Client.Send(stream.ToBytes()))
                    {
                        this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                        return true;
                    }
                    return false;
                }
                else
                {
                    int num = (int)Math.Ceiling(stream.Length / (decimal)size);
                    for (int i = 0; i < num; i++)
                    {
                        if (!this._Client.Send(stream.ToBytes(i, size)))
                        {
                            return false;
                        }
                    }
                    this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
                    return true;
                }
            }

        }


    }
}
