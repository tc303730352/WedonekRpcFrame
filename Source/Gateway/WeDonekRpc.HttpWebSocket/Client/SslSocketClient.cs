using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Threading;

using WeDonekRpc.HttpWebSocket.Interface;

using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpWebSocket.Client
{
    internal class SslSocketClient : BasicSocketClient
        {
                private readonly SslStream _Stream = null;

                private byte[] myByte = null;
                private int _error = 0;
                public override void InitSocket()
                {
                }
                public SslSocketClient(Socket socket, IWebSocketClient client, ISocketServer server) : base(socket, client)
                {
                        this._Stream = new SslStream(new NetworkStream(socket));
                        this._Stream.BeginAuthenticateAsServer(server.Config.Certificate, false, SslProtocols.Tls, false, new AsyncCallback(this._AuthSuccess), null);
                }

                private void _AuthSuccess(IAsyncResult ar)
                {
                        try
                        {
                                this._Stream.EndAuthenticateAsServer(ar);
                                this._Socket.ReceiveBufferSize = 8192;
                                this._Socket.SendBufferSize = 8192;
                                this._BeginReceive();
                        }
                        catch (Exception)
                        {
                                this.Close();
                        }
                }

                private  void _BeginReceive()
                {
                        this.myByte = this._Socket.Available != 0 ? (new byte[this._Socket.Available]) : (new byte[1]);
                        this._Stream.BeginRead(this.myByte, 0, this.myByte.Length, new AsyncCallback(this._Receive), null);
                }
                private void _Receive(IAsyncResult ar)
                {
                        try
                        {
                                int replyNum = this._Stream.EndRead(ar);
                                if (replyNum == 0)
                                {
                                        if (Interlocked.Increment(ref this._error) >= 2)
                                        {
                                                this.Close();
                                        }
                                        return;
                                }
                                this._error = 0;
                                base._ReceiveData(this.myByte, replyNum);
                                this.myByte = this._Socket.Available != 0 ? (new byte[this._Socket.Available]) : (new byte[1]);
                        }
                        catch
                        {
                                this.Close();
                        }
                        finally
                        {
                                if (this.IsCon)
                                {
                                        try
                                        {
                                                this._Stream.BeginRead(this.myByte, 0, this.myByte.Length, new AsyncCallback(this._Receive), null);
                                        }
                                        catch (Exception)
                                        {
                                                this.Close();
                                        }
                                }
                        }
                }
                public override bool Send(byte[] data)
                {
                        try
                        {
                                this._Stream.BeginWrite(data, 0, data.Length, new AsyncCallback(this._SendCallback), null);
                                return true;
                        }
                        catch (Exception e)
                        {
                                this.Close();
                                return false;
                        }
                }
                public override void Dispose()
                {
                        try
                        {
                                this._Stream.Dispose();
                        }
                        finally
                        {
                                base.Dispose();
                        }
                }
                private void _SendCallback(IAsyncResult ar)
                {
                        try
                        {
                                this._Stream.EndWrite(ar);
                        }
                        catch
                        {
                                this.Close();
                        }
                }
        }
}
