using System.Net.Sockets;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Enum;
using WeDonekRpc.TcpClient.Interface;
using WeDonekRpc.TcpClient.Model;
using WeDonekRpc.TcpClient.SystemAllot;

namespace WeDonekRpc.TcpClient.Client
{
    /// <summary>
    /// Socket客户端信息
    /// </summary>
    internal class ClientInfo : Interface.IClientEvent, Interface.IClient
    {
        public SocketClient Client { get; } = null;

        public string ServerId { get; }

        private volatile int _LastTime = HeartbeatTimeHelper.HeartbeatTime;

        public int LastTime => this._LastTime;

        private volatile ClientStatus _ClientStatus = ClientStatus.未连接;

        public ClientStatus ClientStatus => this._ClientStatus;


        /// <summary>
        /// 检查当前客户端是否可以用于发送数据包
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool CheckIsSend ()
        {
            if ( this._ClientStatus == ClientStatus.等待发送 && Interlocked.CompareExchange(ref this._IsSendData, 1, 0) == 0 )
            {
                this._LastTime = HeartbeatTimeHelper.HeartbeatTime;
                return true;
            }
            return false;
        }
        private int _IsSendData = 0;

        /// <summary>
        /// 链接完成
        /// </summary>
        public void ConnectComplateEvent ()
        {
            if ( this._ClientStatus == ClientStatus.正在连接 )
            {
                this._ClientStatus = ClientStatus.链接成功;
            }
        }
        /// <summary>
        /// 授权完成
        /// </summary>
        public void AuthorizationComplate ()
        {
            if ( this._ClientStatus == ClientStatus.链接成功 )
            {
                _ = Interlocked.Exchange(ref this._IsSendData, 0);
                this._ClientStatus = ClientStatus.等待发送;
            }
        }
        internal ClientInfo ( string serverId )
        {
            this.Client = new SocketClient(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            {
                Client = this
            };
            this.ServerId = serverId;
            this._ClientStatus = ClientStatus.未连接;
        }

        public bool ConnectServer ( string ipAddress, int port )
        {
            if ( this._ClientStatus == ClientStatus.未连接 )
            {
                this._ClientStatus = ClientStatus.正在连接;
                if ( !this.Client.ConnectServer(ipAddress, port) )
                {
                    this._ClientStatus = ClientStatus.以关闭;
                    return false;
                }
                return true;
            }
            return false;
        }
        public virtual void AllotEvent ( DataPageInfo page )
        {
            GetDataPage data = new GetDataPage
            {
                Content = page.Content,
                DataType = page.DataType,
                PageType = page.PageType,
                Type = page.Type,
                Client = this,
                IsCompression = page.IsCompression,
                PageId = page.DataId,
                Allot = _GetAllot(page.PageType)
            };
            if ( Config.SocketConfig.IsSyncRead )
            {
                SocketTools.ReadPage(data);
            }
            else
            {
                _ = ThreadPool.UnsafeQueueUserWorkItem(SocketTools.ReadPage, data, true);
            }
        }
        private static IAllot _GetAllot ( byte type )
        {
            if ( type == ConstDicConfig.ReplyPageType )
            {
                return new ReplyAllot();
            }
            else if ( type == ConstDicConfig.PingPageType )
            {
                return new PingAllot();
            }
            else if ( ( ConstDicConfig.DataPageType & type ) == ConstDicConfig.DataPageType )
            {
                return Config.SocketConfig.Allot;
            }
            return new SysAllot();
        }
        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="objPage"></param>
        public bool Send ( DataPage page )
        {
            return this._ClientStatus != ClientStatus.以关闭 && this.Client.Send(page);
        }

        public virtual void ConCloseEvent ()
        {
            this._ClientStatus = ClientStatus.以关闭;
            Manage.ClientManage.ClientClose(this);
        }
        public bool SendPage ( Page page, out string error )
        {
            if ( this._ClientStatus == ClientStatus.以关闭 )
            {
                error = "socket.client.already.close";
                return false;
            }
            else if ( !this.Client.Send(Page.GetDataPage(page)) )
            {
                error = "socket.send.error";
                return false;
            }
            else
            {
                error = null;
                return true;
            }
        }

        public void SendPageErrorEvent ( uint pageId )
        {
            Manage.PageManage.SendError(pageId, "socket.send.error");
        }

        public void SendPageComplateEvent ( uint pageId )
        {
            _ = Interlocked.CompareExchange(ref this._IsSendData, 0, 1);
            if ( pageId != 0 )
            {
                Manage.PageManage.SendSuccess(pageId);
            }
        }
        public void CloseClientCon ()
        {
            if ( this._ClientStatus != ClientStatus.以关闭 )
            {
                this.Client.Close();
            }
        }
        public void ConError ()
        {
            this._ClientStatus = ClientStatus.以关闭;
            Manage.ClientManage.ClientClose(this);
        }
        public void CloseClientCon ( int time )
        {
            if ( this._ClientStatus != ClientStatus.以关闭 )
            {
                this.Client.Close(time);
            }
        }
        public void SendSystemPage ( Page page )
        {
            if ( !this.SendPage(page, out string error) )
            {
                new LogInfo(error, LogGrade.ERROR, "Socket_Client")
                {
                    LogTitle = "发送系统包错误!",
                    LogContent = page.ToJson()
                }.Save();
            }
        }
    }
}
