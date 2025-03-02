using System.Net;
using System.Net.Sockets;

using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket.Client
{
    internal class BasicSocketClient : lSocketClient
    {
        protected readonly Socket _Socket = null;
        private readonly IWebSocketClient _SocketEvent = null;
        private volatile bool _IsCon = true;

        private volatile int _AuthorizeStatus = 0;

        private DataPageInfo _CurrentPage = null;

        public BasicSocketClient (Socket socket, IWebSocketClient client)
        {
            this._SocketEvent = client;
            this._Socket = socket;
        }

        /// <summary>
        /// 远程客户端地址
        /// </summary>
        /// <returns></returns>
        public IPEndPoint RemoteIp
        {
            get
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

        public bool IsCon => this._IsCon;

        public bool IsAuthorize => this._AuthorizeStatus != 0;


        /// <summary>
        /// 初始化Socket连接
        /// </summary>
        public virtual void InitSocket ()
        {

        }

        public void ReceiveComplate ()
        {
            if (this._CurrentPage != null)
            {
                if (this._CurrentPage.LoadProgress == PageLoadProgress.加载完成)
                {
                    this._SocketEvent.Receive(this._CurrentPage);
                }
                this._CurrentPage = null;
            }
        }

        private HttpHead _Head = null;

        protected void _ReceiveData (byte[] myByte, int num)
        {
            if (this._AuthorizeStatus == 0)
            {
                if (!SocketTools.AnalysisWebPage(myByte, ref this._Head, num))
                {
                    this.Close();
                    return;
                }
                else if (this._Head.IsEnd)
                {
                    if (!this._SocketEvent.CheckAuthorize(this._Head, this.RemoteIp))
                    {
                        this.Close();
                        return;
                    }
                    this._SocketEvent.AuthorizeComplate();
                }
            }
            else
            {
                SocketTools.AnalysisPage(myByte, num, ref this._CurrentPage, this);
            }
        }
        public virtual bool Send (byte[] data)
        {
            return true;
        }
        public void Authorize ()
        {
            if (this._AuthorizeStatus == 0)
            {
                this._AuthorizeStatus = 1;
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public bool Close (int time)
        {
            if (!this._IsCon)
            {
                return false;
            }
            this._IsCon = false;
            try
            {
                this._Socket.Close(time);
                return true;
            }
            finally
            {
                this._SocketEvent.ConCloseEvent();
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close ()
        {
            if (!this._IsCon)
            {
                return;
            }
            this._IsCon = false;
            try
            {
                this._Socket.Close();
            }
            finally
            {
                this._SocketEvent.ConCloseEvent();
            }
        }

        public virtual void Dispose ()
        {
            try
            {
                this._Socket.Dispose();
            }
            catch
            { }
        }
    }
}
