using System;
using System.Net.Sockets;

using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket.Client
{
    internal class SocketClient : BasicSocketClient
    {
        private byte[] myByte = null;
        public SocketClient (Socket socket, IWebSocketClient client) : base(socket, client)
        {
        }
        public override void InitSocket ()
        {
            try
            {
                this._Socket.ReceiveBufferSize = 8192;
                this._Socket.SendBufferSize = 8192;
                this._BeginReceive();
            }
            catch (Exception)
            {
                this.Close();
            }
        }
        protected void _BeginReceive ()
        {
            this.myByte = this._Socket.Available != 0 ? ( new byte[this._Socket.Available] ) : ( new byte[550] );
            _ = this._Socket.BeginReceive(this.myByte, 0, this.myByte.Length, SocketFlags.None, new AsyncCallback(this._Receive), null);
        }

        private int _error = 0;
        private void _Receive (IAsyncResult ar)
        {
            try
            {
                int replyNum = this._Socket.EndReceive(ar, out SocketError error);
                if (error != SocketError.Success || replyNum == 0)
                {
                    if (++this._error >= 2)
                    {
                        this.Close();
                    }
                    return;
                }
                this._error = 0;
                this._ReceiveData(this.myByte, replyNum);
                this.myByte = this._Socket.Available != 0 ? ( new byte[this._Socket.Available] ) : ( new byte[100] );
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
                if (this.IsCon)
                {
                    try
                    {
                        _ = this._Socket.BeginReceive(this.myByte, 0, this.myByte.Length, SocketFlags.None, new AsyncCallback(this._Receive), null);
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
        public override bool Send (byte[] data)
        {
            try
            {
                _ = this._Socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(this._SendCallback), null);
                return true;
            }
            catch (SocketException)
            {
                this.Close();
                return false;
            }
            catch (Exception)
            {
                this.Close();
                return false;
            }
        }

        private void _SendCallback (IAsyncResult ar)
        {
            try
            {
                _ = this._Socket.EndSend(ar);

            }
            catch (Exception)
            {
                this.Close();
            }
        }
    }
}
