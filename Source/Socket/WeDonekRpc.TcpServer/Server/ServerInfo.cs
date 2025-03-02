using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using WeDonekRpc.TcpServer.Interface;
using WeDonekRpc.TcpServer.Log;

namespace WeDonekRpc.TcpServer.Server
{
    internal class ServerInfo
    {
        /// <summary>
        /// 侦听的线程
        /// </summary>
        private Thread ListenThread;

        /// <summary>
        /// 客户端的管理类
        /// </summary>
        private readonly Manage.ClientManage _ClientInfo = null;

        protected Manage.ClientManage ClientInfo => this._ClientInfo;

        internal ServerInfo (Interface.IAllot allot, Interface.ISocketEvent socketEvent, string publicKey)
        {
            this._PublicKey = publicKey;
            this._ClientInfo = new Manage.ClientManage(allot, socketEvent);
        }

        private readonly string _PublicKey = null;

        internal string PublicKey => this._PublicKey;

        internal ServerInfo ()
        {
            this._ClientInfo = new Manage.ClientManage();
        }

        internal int ClientCount => this._ClientInfo.ClientCount;


        internal IClient FindClient (string bindParam)
        {
            return this._ClientInfo.Find(bindParam);
        }

        private int _Port = 0;

        internal int Port => this._Port;

        /// <summary>
        /// 服务的Socket
        /// </summary>
        private Socket _Socket;

        protected Socket Socket => this._Socket;

        /// <summary>
        /// 服务的唯一ID
        /// </summary>
        private readonly Guid _ServerId = Guid.NewGuid();

        /// <summary>
        /// 是否暂停服务
        /// </summary>
        private bool _IsStop = false;

        /// <summary>
        /// 服务的ID
        /// </summary>
        internal Guid ServerId => this._ServerId;

        internal void CloseServer ()
        {
            this._ClientInfo.CloseClient();
            this._IsStop = true;
            this._Socket.Close(1);
        }

        /// <summary>
        /// 初始化服务
        /// </summary>
        /// <param name="port"></param>
        internal void InitServer (int port)
        {
            this._Port = port;
            this._Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            System.Net.IPAddress objIP = IPAddress.Any;
            IPEndPoint objIPendPoint = new IPEndPoint(objIP, port);
            this._Socket.Bind(objIPendPoint);
            this._Socket.Listen(50000);
            this.ListenThread = new Thread(new ThreadStart(this.BeginListen));
            this.ListenThread.Start();
        }

        internal void CheckIsCon ()
        {
            this._ClientInfo?.CheckIsCon();
        }

        internal void CloseClient (Guid clientId)
        {
            this._ClientInfo?.CloseClient(clientId);
        }


        internal void ClientCloseEvent (Guid clientId)
        {
            this._ClientInfo.ClientCloseEvent(clientId);
        }

        /// <summary>
        /// 开始监听
        /// </summary>
        protected internal virtual void BeginListen ()
        {
            do
            {
                try
                {
                    Client.SocketClient temp = new Client.SocketClient(this._Socket.Accept(), this._Port);
                    this._ClientInfo.AddClient(this._ServerId, temp);
                }
                catch (SocketException e)
                {
                    IoLogSystem.AddListenLog(e);
                }
                catch (Exception e)
                {
                    IoLogSystem.AddListenLog(e);
                }
            } while (!this._IsStop);
        }
    }
}
