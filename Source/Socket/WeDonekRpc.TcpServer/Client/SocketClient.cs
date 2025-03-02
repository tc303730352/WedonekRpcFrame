using System;
using System.Net;
using System.Net.Sockets;
using WeDonekRpc.IOBuffer;
using WeDonekRpc.IOBuffer.Interface;
using WeDonekRpc.TcpServer.Log;

namespace WeDonekRpc.TcpServer.Client
{
    /// <summary>
    /// 客户端
    /// </summary>
    internal class SocketClient
    {
        private readonly Socket _Socket = null;
        private ISocketBuffer _Buffer = null;
        private int _Ver = 0;
        private int _Min = 8192;

        internal SocketClient(Socket socket, int port)
        {
            this.Port = port;
            this._Socket = socket;
        }
        internal int Port { get; }
        /// <summary>
        /// 客户端ID
        /// </summary>
        internal Guid ClientId { get; } = Guid.NewGuid();

        public IPEndPoint RemoteIp { get; private set; }

        /// <summary>
        /// 客户端
        /// </summary>
        internal Interface.IClientEvent Client { get; set; }
        /// <summary>
        /// 异步发送字节流
        /// </summary>
        /// <param name="buffer"></param>
        private bool _Send(ISocketBuffer buffer)
        {
            try
            {
                _ = this._Socket.BeginSend(buffer.Stream, 0, buffer.BufferSize, 0, new AsyncCallback(this._SendCallback), buffer);
                return true;
            }
            catch (SocketException e)
            {
                buffer.Dispose();
                IoLogSystem.AddSendLog(e, this);
                this.Close();
                return false;
            }
            catch (Exception e)
            {
                buffer.Dispose();
                IoLogSystem.AddSendLog(e, this);
                this.Close();
                return false;
            }
        }
        internal bool Send(ref Model.DataPage page)
        {
            ISocketBuffer buffer = SocketHelper.GetSendBuffer(ref page);
            return this._Send(buffer);
        }
        internal bool Send(ref Model.DataPage page, uint pageId)
        {
            ISocketBuffer buffer = SocketHelper.GetSendBuffer(ref page, pageId);
            return this._Send(buffer);
        }

        private void _SendCallback(IAsyncResult ar)
        {
            ISocketBuffer buffer = (ISocketBuffer)ar.AsyncState;
            int ver = buffer.Ver;
            try
            {
                _ = this._Socket.EndSend(ar);
                this.Client.SendPageComplateEvent(buffer.PageId);
            }
            catch (SocketException e)
            {
                this.Client.SendPageErrorEvent(buffer.PageId);
                IoLogSystem.AddSendLog(e, this);
                this.Close();
                return;
            }
            catch (Exception e)
            {
                this.Client.SendPageErrorEvent(buffer.PageId);
                IoLogSystem.AddSendLog(e, this);
                this.Close();
            }
            finally
            {
                buffer.Dispose(ver);
            }
        }

        /// <summary>
        /// 初始化Socket连接
        /// </summary>
        public void InitSocket()
        {
            this._Min = Config.SocketConfig.MinBufferSize;
            this._Socket.ReceiveBufferSize = this._Min;
            this._Socket.SendBufferSize = Config.SocketConfig.SendBufferSize;
            this._Buffer = BufferCollect.ApplyBuffer(this._Min, ref this._Ver);
            try
            {
                this.RemoteIp = (IPEndPoint)this._Socket.RemoteEndPoint;
                _ = this._Socket.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
            }
            catch (SocketException e)
            {
                this._Buffer.Dispose();
                IoLogSystem.AddInitLog(e, this);
                this.Close();
            }
            catch (Exception e)
            {
                this._Buffer.Dispose();
                IoLogSystem.AddInitLog(e, this);
                this.Close();
            }
        }

        /// <summary>
        /// 当前正在接收的包信息
        /// </summary>
        private Model.DataPageInfo _CurrentPage;

        private volatile bool _IsCon = true;
        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="evt"></param>
        internal void ReturnInfo(IAsyncResult evt)
        {
            try
            {
                int size = this._Socket.EndReceive(evt, out SocketError error);
                if (error == SocketError.ConnectionReset || size == 0)
                {
                    this.Close();
                    return;
                }
                if (!SocketHelper.SplitPage(this._Buffer.Stream, ref size, ref this._CurrentPage, this))
                {
                    IoLogSystem.AddErrorPageLog(this._Buffer, this, this._CurrentPage);
                    this.Close();
                    return;
                }
                this._Buffer = BufferCollect.ApplyBuffer(this._Socket.Available, this._Buffer, ref this._Min, ref this._Ver);
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
                if (this._IsCon)
                {
                    try
                    {
                        _ = this._Socket.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
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
        }
        /// <summary>
        /// 接收数据完成时调用
        /// </summary>
        internal void ReceiveComplate()
        {
            this.Client.AllotEvent(this._CurrentPage);
            this._CurrentPage = null;
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        internal void Close()
        {
            this._IsCon = false;
            this._CurrentPage = null;
            IoLogSystem.AddConClose(this, this._Socket);
            try
            {
                this.Client?.ConCloseEvent();
                if (this._Socket != null)
                {
                    this._Buffer?.Dispose(this._Ver);
                    this._Socket.Close();
                    this._Socket.Dispose();
                }
            }
            catch
            { }
        }

        /// <summary>
        /// 关闭连接
        /// </summary>
        internal void Close(int time)
        {
            this._IsCon = false;
            IoLogSystem.AddConClose(this, this._Socket, time);
            try
            {
                this.Client?.ConCloseEvent();
                if (this._Socket != null)
                {
                    this._Buffer?.Dispose(this._Ver);
                    this._Socket.Close(time);
                }
            }
            catch
            { }
        }

    }
}
