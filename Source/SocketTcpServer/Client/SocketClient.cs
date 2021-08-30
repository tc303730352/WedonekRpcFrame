using System;
using System.Net;
using System.Net.Sockets;

using SocketBuffer;

namespace SocketTcpServer.Client
{
        /// <summary>
        /// 客户端
        /// </summary>
        internal class SocketClient
        {
                private readonly Socket _Socket = null;
                private readonly string _RemoteIp = null;
                internal SocketClient(Socket socket)
                {
                        this._Socket = socket;
                        this._RemoteIp = socket.RemoteEndPoint.ToString();
                }

                private Guid _ClientId = Guid.NewGuid();
                /// <summary>
                /// 客户端ID
                /// </summary>
                internal Guid ClientId => this._ClientId;

                private Interface.IClientEvent _Client;

                /// <summary>
                /// 客户端
                /// </summary>
                internal Interface.IClientEvent Client
                {
                        set => this._Client = value;
                }
                /// <summary>
                /// 异步发送字节流
                /// </summary>
                /// <param name="dataStream"></param>
                private bool _Send(ISocketBuffer buffer)
                {
                        try
                        {
                                this._Socket.BeginSend(buffer.Stream, 0, buffer.BufferSize, 0, new AsyncCallback(this._SendCallback), buffer);
                                return true;
                        }
                        catch (SocketException)
                        {
                                this.Close();
                                buffer.Dispose();
                                return false;
                        }
                        catch (Exception)
                        {
                                this.Close();
                                buffer.Dispose();
                                return false;
                        }
                }
                internal bool Send(Model.DataPage page)
                {
                        ISocketBuffer buffer = SocketHelper.GetSendBuffer(page);
                        return this._Send(buffer);
                }

                private void _SendCallback(IAsyncResult ar)
                {
                        ISocketBuffer buffer = (ISocketBuffer)ar.AsyncState;
                        int ver = buffer.Ver;
                        try
                        {
                                this._Socket.EndSend(ar);
                                this._Client.SendPageComplateEvent(buffer.PageId);
                        }
                        catch (SocketException)
                        {
                                this._Client.SendPageErrorEvent(buffer.PageId, "socket.send.error");
                                this.Close();
                                return;
                        }
                        catch (Exception)
                        {
                                this._Client.SendPageErrorEvent(buffer.PageId, "socket.send.error");
                                this.Close();
                        }
                        finally
                        {
                                buffer.Dispose(ver);
                        }
                }
                private ISocketBuffer _Buffer = null;
                private int _Ver = 0;
                /// <summary>
                /// 初始化Socket连接
                /// </summary>
                public void InitSocket()
                {
                        this._Socket.ReceiveBufferSize = 8192;
                        this._Socket.SendBufferSize = 8192;
                        this._Buffer = BufferCollect.ApplyBuffer(256, ref this._Ver);
                        try
                        {
                                this._Socket.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
                        }
                        catch (Exception)
                        {
                                this._Buffer.Dispose();
                                this.Close();
                        }
                }

                /// <summary>
                /// 当前正在接收的包信息
                /// </summary>
                private Model.DataPageInfo _CurrentPage;

                private volatile bool _IsCon = true;
                private int _Error = 0;

                /// <summary>
                /// 异步回调
                /// </summary>
                /// <param name="evt"></param>
                internal void ReturnInfo(IAsyncResult evt)
                {
                        try
                        {
                                if (!this._IsCon)
                                {
                                        return;
                                }
                                int size = this._Socket.EndReceive(evt, out SocketError error);
                                if (size == 0 || error != SocketError.Success)
                                {
                                        if (error == SocketError.ConnectionReset || ++this._Error >= 2)
                                        {
                                                this.Close();
                                        }
                                        return;
                                }
                                this._Error = 0;
                                if (!SocketHelper.SplitPage(this._Buffer.Stream, size, ref this._CurrentPage, this))
                                {
                                        this.Close();
                                        return;
                                }
                                this._Buffer = BufferCollect.ApplyBuffer(this._Socket.Available, this._Buffer, ref this._Ver);
                        }
                        catch (SocketException)
                        {
                                this.Close();
                        }
                        catch (Exception)
                        {
                                this.Close();
                        }
                        finally
                        {
                                if (this._IsCon)
                                {
                                        try
                                        {
                                                this._Socket.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
                                        }
                                        catch (SocketException)
                                        {
                                                this.Close();
                                        }
                                        catch (Exception)
                                        {
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
                        if (this._CurrentPage != null)
                        {
                                this._Client.AllotEvent(this._CurrentPage);
                                this._CurrentPage = null;
                        }
                }
                /// <summary>
                /// 关闭连接
                /// </summary>
                internal void Close()
                {
                        this._IsCon = false;
                        this._CurrentPage = null;
                        try
                        {
                                if (this._Client != null)
                                {
                                        this._Client.ConCloseEvent();
                                }
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
                        try
                        {
                                if (this._Client != null)
                                {
                                        this._Client.ConCloseEvent();
                                }
                                if (this._Socket != null)
                                {
                                        this._Buffer?.Dispose(this._Ver);
                                        this._Socket.Close(time);
                                }
                        }
                        catch
                        { }
                }



                internal IPEndPoint GetClientAddress()
                {
                        try
                        {
                                return (IPEndPoint)this._Socket.RemoteEndPoint;
                        }
                        catch
                        { }
                        return null;
                }
        }
}
