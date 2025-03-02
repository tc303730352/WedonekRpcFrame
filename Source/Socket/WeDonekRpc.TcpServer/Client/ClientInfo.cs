using System;
using System.Net;
using System.Threading;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpServer.Config;
using WeDonekRpc.TcpServer.Enum;
using WeDonekRpc.TcpServer.Model;
using WeDonekRpc.TcpServer.SystemAllot;

namespace WeDonekRpc.TcpServer.Client
{
    /// <summary>
    /// Socket客户端信息
    /// </summary>
    internal class ClientInfo : Interface.IClientEvent, Interface.IClient
    {
        private readonly SocketClient _Client;
        /// <summary>
        /// 客户端Socket对象
        /// </summary>
        internal SocketClient Client => this._Client;

        private string _BindParam = null;

        private readonly Interface.ISocketEvent _Event = null;

        /// <summary>
        /// Socket事件
        /// </summary>
        public Interface.ISocketEvent Event => this._Event;


        public Guid ClientId
        {
            get
            {
                if (this._Client != null)
                {
                    return this._Client.ClientId;
                }
                return Guid.Empty;
            }
        }
        private readonly DateTime _ConTime = DateTime.Now;

        private volatile ClientStatus _CurrentStatus = ClientStatus.未连接;

        public ClientStatus CurrentStatus => this._CurrentStatus;

        /// <summary>
        /// 是否完成了授权
        /// </summary>
        private bool _IsAuthorization = false;

        private readonly Guid _ServerId = Guid.Empty;
        /// <summary>
        /// 服务端ID
        /// </summary>
        public Guid ServerId => this._ServerId;

        public string BindParam => this._BindParam;


        public bool IsAuthorization { get => this._IsAuthorization; }

        public void CheckIsCon (DateTime time)
        {
            if (this.CurrentStatus != ClientStatus.已关闭 && this.CurrentStatus != ClientStatus.未连接)
            {
                if (this._IsAuthorization == false && this._ConTime <= time)
                {
                    this.Client.Close();
                    return;
                }
                this._Send(Page.GetSysPage("Heartbeat", string.Empty));
            }
        }
        public bool CheckIsCon ()
        {
            if (this.CurrentStatus == ClientStatus.已连接)
            {
                return this._IsAuthorization;
            }
            return false;
        }

        /// <summary>
        /// 处理数据的接口
        /// </summary>
        private readonly Interface.IAllot objAllot;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="client"></param>
        /// <param name="allot"></param>
        /// <param name="socketEvent"></param>
        internal ClientInfo (Guid serverId, SocketClient client, Interface.IAllot allot, Interface.ISocketEvent socketEvent)
        {
            this.objAllot = allot;
            this._ServerId = serverId;
            this._Event = socketEvent;
            this._Client = client;
            this._Client.Client = this;
            this._Client.InitSocket();
            this._CurrentStatus = Enum.ClientStatus.已连接;
        }
        public void Welcome ()
        {
            this._Send(Page.GetSysPage("Welcome", string.Empty));
        }
        public virtual void AllotEvent (DataPageInfo page)
        {
            if (!this._IsAuthorization && page.PageType != ConstDicConfig.SystemPageType)
            {
                this.CloseClientCon(2);
                return;
            }
            GetDataPage data = new GetDataPage
            {
                Content = page.Content,
                DataType = page.DataType,
                PageType = page.PageType,
                Type = page.Type,
                Client = this,
                IsCompression = page.IsCompression,
                PageId = page.DataId,
                Allot = this.ChioseAllot(page.PageType)
            };
            if (SocketConfig.IsSyncRead)
            {
                SocketHelper.ReadPage(data);
            }
            else
            {
                _ = ThreadPool.UnsafeQueueUserWorkItem(SocketHelper.ReadPage, data, true);
            }
        }
        private static readonly PingAllot _PingAllot = new PingAllot();
        protected virtual Interface.IAllot ChioseAllot (byte pageType)
        {
            if (pageType == ConstDicConfig.PingPageType)
            {
                return _PingAllot;
            }
            else if (( ConstDicConfig.DataPageType & pageType ) == ConstDicConfig.DataPageType)
            {
                return (Interface.IAllot)this.objAllot.Clone();
            }
            else if (pageType == ConstDicConfig.SystemPageType)
            {
                return new SystemAllot.AllotInfo();
            }
            else
            {
                return new FileUp.Allot.FileAllot();
            }
        }

        public bool Send (Page page, out string error)
        {
            DataPage.InitDataPage(page, out DataPage data);
            if (!this.Client.Send(ref data))
            {
                error = "socket.send.error";
                return false;
            }
            error = null;
            return true;
        }
        public virtual void ConCloseEvent ()
        {
            this._CurrentStatus = Enum.ClientStatus.已关闭;
            TcpServer.ClientCloseEvent(this.ServerId, this.Client.ClientId);
        }

        public void SendPageErrorEvent (uint pageId)
        {
            if (pageId != uint.MinValue)
            {
                Manage.ReplyPageManage.SendError(pageId);
            }
        }
        public void _Send (Page page)
        {
            DataPage.InitDataPage(page, out DataPage data);
            _ = this.Client.Send(ref data);
        }
        public bool Send (ReplyPage page)
        {
            DataPage.InitDataPage(page.page, out DataPage data);
            return this.Client.Send(ref data, page.id);
        }

        public void AuthorizationComplate (string bindParam)
        {
            this._BindParam = bindParam;
            this._IsAuthorization = true;
        }

        public void SendPageComplateEvent (uint pageId)
        {
            if (pageId != uint.MinValue)
            {
                Manage.ReplyPageManage.ComplateSend(pageId);
            }
        }


        public void CloseClientCon ()
        {
            this._Client.Close();
        }

        public void CloseClientCon (int time)
        {
            this._Client.Close(time);
        }

        public IPEndPoint RempteIp
        {
            get
            {
                return this._Client.RemoteIp;
            }
        }

    }
}
