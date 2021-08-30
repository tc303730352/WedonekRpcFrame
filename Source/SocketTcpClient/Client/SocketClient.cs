using System;
using System.Net;
using System.Net.Sockets;

using SocketBuffer;

namespace SocketTcpClient.Client
{
        /// <summary>
        /// 客户端
        /// </summary>
        internal class SocketTcpClient : System.Net.Sockets.Socket
        {
                internal SocketTcpClient(SocketInformation socketInformation)
                    : base(socketInformation)
                {

                }

                internal SocketTcpClient(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType) :
                    base(addressFamily, socketType, protocolType)
                {

                }
                private string _LocalIp = null;
                private Guid _ClientId = Guid.NewGuid();
                private ISocketBuffer _Buffer = null;
                private int _ver = 0;
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
                private bool Send(ISocketBuffer buffer)
                {
                        try
                        {
                                this.BeginSend(buffer.Stream, 0, buffer.BufferSize, 0, new AsyncCallback(this._SendCallback), buffer);
                                return true;
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
                        ISocketBuffer buffer = SocketTools.GetSendBuffer(page);
                        return this.Send(buffer);
                }
                public bool ConnectServer(string ipAddress, int port)
                {
                        IPAddress ip = IPAddress.Parse(ipAddress);
                        try
                        {
                                this.Connect(ip, port);
                                if (this.InitSocket())
                                {
                                        this._Client.ConnectComplateEvent();
                                }
                                else
                                {
                                        this.Close();
                                }
                                //this.BeginConnect(objIp, new AsyncCallback(this.ConnectCallback), null);
                                return true;
                        }
                        catch (SocketException)
                        {
                                this.Close();
                                return false;
                        }
                }
                private void ConnectCallback(IAsyncResult ar)
                {
                        if (ar.IsCompleted)
                        {
                                try
                                {
                                        this.EndConnect(ar);
                                }
                                catch (SocketException)
                                {
                                        this.Close();
                                        return;
                                }
                                catch (Exception)
                                {
                                        this.Close();
                                        return;
                                }
                                if (this.InitSocket())
                                {
                                        this._Client.ConnectComplateEvent();
                                }
                                else
                                {
                                        this.Close();
                                }
                        }
                        else
                        {
                                this.Close();
                        }
                }
                private void _SendCallback(IAsyncResult ar)
                {
                        ISocketBuffer buffer = (ISocketBuffer)ar.AsyncState;
                        int ver = buffer.Ver;
                        try
                        {
                                this.EndSend(ar);
                                this._Client.SendPageComplateEvent(buffer.PageId);
                        }
                        catch (SocketException)
                        {
                                this._Client.SendPageErrorEvent(buffer.PageId);
                                this.Close();
                                return;
                        }
                        catch (Exception)
                        {
                                this._Client.SendPageErrorEvent(buffer.PageId);
                                this.Close();
                                return;
                        }
                        finally
                        {
                                buffer.Dispose(ver);
                        }
                }


                /// <summary>
                /// 初始化Socket连接
                /// </summary>
                private bool InitSocket()
                {
                        this.ReceiveBufferSize = 8192;
                        this.SendBufferSize = 8192;
                        this._LocalIp = this.LocalEndPoint.ToString();
                        this._Buffer = BufferCollect.ApplyBuffer(256, ref this._ver);
                        try
                        {
                                this.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
                                return true;
                        }
                        catch (Exception)
                        {
                                this.Close();
                                return false;
                        }
                }


                private Model.DataPageInfo _CurrentPage;
                private volatile bool _IsCon = true;
                private int _error = 0;
                /// <summary>
                /// 异步回调
                /// </summary>
                /// <param name="evt"></param>
                internal void ReturnInfo(IAsyncResult evt)
                {
                        SocketError objError = SocketError.Success;
                        try
                        {
                                if (!this._IsCon)
                                {
                                        return;
                                }
                                int size = this.EndReceive(evt, out objError);
                                if (size == 0 || objError != SocketError.Success)
                                {
                                        if (objError == SocketError.ConnectionReset || ++this._error >= 2)
                                        {
                                                this.Close();
                                        }
                                        return;
                                }
                                this._error = 0;
                                if (!SocketTools.SplitPage(this._Buffer.Stream, size, ref this._CurrentPage, this))
                                {
                                        this.Close();
                                        return;
                                }
                                this._Buffer = BufferCollect.ApplyBuffer(this.Available, this._Buffer, ref this._ver);
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
                                try
                                {
                                        if (this._IsCon)
                                        {
                                                this.BeginReceive(this._Buffer.Stream, 0, this._Buffer.BufferSize, SocketFlags.None, new AsyncCallback(this.ReturnInfo), null);
                                        }
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

                /// <summary>
                /// 接收数据完成时调用
                /// </summary>
                internal void ReceiveComplate()
                {
                        if (this._CurrentPage != null)
                        {
                                if (this._CurrentPage.LoadProgress == Enum.PageLoadProgress.加载完成)
                                {
                                        this._Client.AllotEvent(this._CurrentPage);
                                }
                                this._CurrentPage = null;
                        }
                }
                /// <summary>
                /// 关闭连接
                /// </summary>
                public new void Close()
                {
                        this._IsCon = false;
                        if (this._Client != null)
                        {
                                this._Client.ConCloseEvent();
                        }
                        this._Buffer?.Dispose(this._ver);
                        base.Close();
                        base.Dispose(true);
                }

                /// <summary>
                /// 关闭连接
                /// </summary>
                public new void Close(int time)
                {
                        this._IsCon = false;
                        if (this._Client != null)
                        {
                                this._Client.ConCloseEvent();
                        }
                        this._Buffer?.Dispose(this._ver);
                        base.Close(time);
                }
                protected override void Dispose(bool disposing)
                {
                        this._IsCon = false;
                        base.Dispose(disposing);
                }
        }
}
