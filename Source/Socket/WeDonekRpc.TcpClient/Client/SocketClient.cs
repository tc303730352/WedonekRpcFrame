using System;
using System.Net;
using System.Net.Sockets;
using WeDonekRpc.IOBuffer;
using WeDonekRpc.IOBuffer.Interface;
using WeDonekRpc.TcpClient.Log;

namespace WeDonekRpc.TcpClient.Client
{
    /// <summary>
    /// 客户端
    /// </summary>
    internal class SocketClient : System.Net.Sockets.Socket
    {
        internal SocketClient (SocketInformation socketInformation)
            : base(socketInformation)
        {

        }

        internal SocketClient (AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) :
            base(addressFamily, socketType, protocolType)
        {

        }

        private ISocketBuffer _Buffer = null;
        private int _ver = 0;
        /// <summary>
        /// 客户端ID
        /// </summary>
        internal Guid ClientId { get; } = Guid.NewGuid();

        private Interface.IClientEvent _Client;

        /// <summary>
        /// 客户端
        /// </summary>
        internal Interface.IClientEvent Client
        {
            set => this._Client = value;
        }
        public EndPoint RemoteIp { get; private set; }

        public EndPoint LocalPoint { get; private set; }
        /// <summary>
        /// 异步发送字节流
        /// </summary>
        /// <param name="buffer"></param>
        private bool Send (ISocketBuffer buffer)
        {
            try
            {
                _ = this.BeginSend(buffer.Stream, 0, buffer.BufferSize, 0, new AsyncCallback(this._SendCallback), buffer);
                return true;
            }
            catch (SocketException e)
            {
                IoLogSystem.AddSendLog(e, this);
                buffer.Dispose();
                this.Close();
                return false;
            }
            catch (Exception e)
            {
                IoLogSystem.AddSendLog(e, this);
                buffer.Dispose();
                this.Close();
                return false;
            }
        }
        internal bool Send (Model.DataPage page)
        {
            ISocketBuffer buffer = SocketTools.GetSendBuffer(page);
            return this.Send(buffer);
        }
        public bool ConnectServer (string ipAddress, int port)
        {
            this.RemoteIp = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            try
            {
                this.Connect(this.RemoteIp);
                if (this.InitSocket())
                {
                    this._Client.ConnectComplateEvent();
                }
                else
                {
                    this.Close();
                }
                return true;
            }
            catch (SocketException e)
            {
                IoLogSystem.AddConnectLog(e, this);
                this.Close();
                return false;
            }
        }
        private void _SendCallback (IAsyncResult ar)
        {
            ISocketBuffer buffer = (ISocketBuffer)ar.AsyncState;
            int ver = buffer.Ver;
            try
            {
                int size = this.EndSend(ar);
                this._Client.SendPageComplateEvent(buffer.PageId);
            }
            catch (SocketException e)
            {
                this._Client.SendPageErrorEvent(buffer.PageId);
                IoLogSystem.AddSendLog(e, this);
                this.Close();
                return;
            }
            catch (Exception e)
            {
                this._Client.SendPageErrorEvent(buffer.PageId);
                IoLogSystem.AddSendLog(e, this);
                this.Close();
                return;
            }
            finally
            {
                buffer.Dispose(ver);
            }
        }

        private int _Min;
        /// <summary>
        /// 初始化Socket连接
        /// </summary>
        private bool InitSocket ()
        {
            this._Min = Config.SocketConfig.MinBufferSize;
            this.ReceiveBufferSize = Config.SocketConfig.ReceiveBufferSize;
            this.SendBufferSize = Config.SocketConfig.SendBufferSize;
            this._Buffer = BufferCollect.ApplyBuffer(this._Min, ref this._ver);
            try
            {
                this.LocalPoint = this.LocalEndPoint;
                _ = this.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
                return true;
            }
            catch (SocketException e)
            {
                IoLogSystem.AddInitLog(e, this);
                this.Close();
                return false;
            }
            catch (Exception e)
            {
                IoLogSystem.AddInitLog(e, this);
                this.Close();
                return false;
            }
        }


        private Model.DataPageInfo _CurrentPage;
        private volatile bool _IsCon = true;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="evt"></param>
        internal void ReturnInfo (IAsyncResult evt)
        {
            try
            {
                int size = this.EndReceive(evt, out SocketError error);
                if (error == SocketError.ConnectionReset || size == 0)
                {
                    this.Close();
                    return;
                }
                if (!SocketTools.SplitPage(this._Buffer.Stream, ref size, ref this._CurrentPage, this))
                {
                    IoLogSystem.AddErrorPageLog(this._Buffer, this, this._CurrentPage);
                    this.Close();
                    return;
                }
                this._Buffer = BufferCollect.ApplyBuffer(this.Available, this._Buffer, ref this._Min, ref this._ver);
            }
            catch (SocketException e)
            {
                IoLogSystem.AddReceiveLog(e, this);
                this.Close();
            }
            catch (Exception e)
            {
                IoLogSystem.AddReceiveLog(e, this);
                this.Close();
            }
            finally
            {
                try
                {
                    if (this._IsCon)
                    {
                        _ = this.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
                    }
                }
                catch (SocketException e)
                {
                    IoLogSystem.AddReceiveLog(e, this);
                    this.Close();
                }
                catch (Exception e)
                {
                    IoLogSystem.AddReceiveLog(e, this);
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 接收数据完成时调用
        /// </summary>
        internal void ReceiveComplate ()
        {
            this._Client.AllotEvent(this._CurrentPage);
            this._CurrentPage = null;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public new void Close ()
        {
            this._IsCon = false;
            IoLogSystem.AddConClose(this);
            try
            {
                this._Client?.ConCloseEvent();
                this._Buffer?.Dispose(this._ver);
                base.Close();
                base.Dispose(true);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        public new void Close (int time)
        {
            this._IsCon = false;
            IoLogSystem.AddConClose(this, time);
            try
            {
                this._Client?.ConCloseEvent();
                this._Buffer?.Dispose(this._ver);
                base.Close(time);
            }
            catch
            {

            }
        }
        protected override void Dispose (bool disposing)
        {
            this._IsCon = false;
            base.Dispose(disposing);
        }
    }
}
