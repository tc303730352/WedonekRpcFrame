using System;
using System.Net;
using System.Threading;

using SocketTcpClient.SystemAllot;

using SocketTcpServer.Enum;
using SocketTcpServer.Model;

namespace SocketTcpServer.Client
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
                private bool _IsComplateAuthorization = false;

                private Guid _ServerId = Guid.Empty;
                /// <summary>
                /// 服务端ID
                /// </summary>
                internal Guid ServerId => this._ServerId;

                public string BindParam => this._BindParam;

                public void CheckIsCon(DateTime time)
                {
                        if (this.CurrentStatus != ClientStatus.已关闭 && this.CurrentStatus != ClientStatus.未连接)
                        {
                                if (this._IsComplateAuthorization == false && this._ConTime <= time)
                                {
                                        this.Client.Close();
                                        return;
                                }
                                this._Send(Page.GetSysPage("Heartbeat", string.Empty));
                        }
                }
                public bool CheckIsCon()
                {
                        if (this.CurrentStatus == ClientStatus.已连接)
                        {
                                return this._IsComplateAuthorization;
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
                /// <param name="client"></param>
                internal ClientInfo(Guid serverId, SocketClient client, Interface.IAllot allot, Interface.ISocketEvent socketEvent)
                {
                        this.objAllot = allot;
                        this._ServerId = serverId;
                        this._Event = socketEvent;
                        this._Client = client;
                        this._Client.Client = this;
                        this._Client.InitSocket();
                        this._CurrentStatus = Enum.ClientStatus.已连接;
                        ThreadPool.UnsafeQueueUserWorkItem((a) =>
                        {
                                this._Send(Page.GetSysPage("Welcome", string.Empty));
                        }, null);
                }
                public virtual void AllotEvent(DataPageInfo page)
                {
                        if (!this._IsComplateAuthorization && page.PageType != PageType.系统包)
                        {
                                this.CloseClientCon(2);
                                return;
                        }
                        ThreadPool.UnsafeQueueUserWorkItem(new WaitCallback(SocketHelper.ReadPage), new GetDataPage
                        {
                                PageContent = page.Content,
                                DataType = page.DataType,
                                PageType = page.PageType,
                                Type = page.Type,
                                ClientId = this._Client.ClientId,
                                ServerId = this.ServerId,
                                Client = this,
                                IsCompression = page.IsCompression,
                                OriginalSize = page.OriginalSize,
                                PageId = page.DataId,
                                Allot = this.ChioseAllot(page.PageType)
                        });
                }
                private static readonly PingAllot _PingAllot = new PingAllot();
                protected virtual Interface.IAllot ChioseAllot(Enum.PageType pageType)
                {
                        if (pageType == PageType.ping包)
                        {
                                return _PingAllot;
                        }
                        else if ((PageType.数据包 & pageType) == PageType.数据包)
                        {
                                return (Interface.IAllot)this.objAllot.Clone();
                        }
                        else if (pageType == PageType.回复包)
                        {
                                return new SystemAllot.ReplyPageAllot();
                        }
                        else if (pageType == PageType.系统包)
                        {
                                return new SystemAllot.AllotInfo();
                        }
                        else
                        {
                                return new FileUp.Allot.FileAllot();
                        }
                }
                /// <summary>
                /// 发送数据包
                /// </summary>
                /// <param name="objPage"></param>
                public void Send(DataPage page)
                {
                        if (this._CurrentStatus != Enum.ClientStatus.已关闭)
                        {
                                this._Client.Send(page);
                        }
                }
                public virtual void ConCloseEvent()
                {
                        this._CurrentStatus = Enum.ClientStatus.已关闭;
                        SocketTcpServer.ClientCloseEvent(this.ServerId, this.Client.ClientId);
                }

                public void SendPageErrorEvent(int pageId, string error)
                {
                        Manage.PageManage.SendError(pageId, error);
                }
                public void _Send(Page page)
                {
                        this.Client.Send(DataPage.GetDataPage(page));
                }
                public bool Send(Page page, out string error)
                {
                        if (!this.Client.Send(DataPage.GetDataPage(page)))
                        {
                                error = "socket.client.send.error";
                                return false;
                        }
                        error = null;
                        return true;
                }

                public void AuthorizationComplate(string bindParam)
                {
                        this._BindParam = bindParam;
                        this._IsComplateAuthorization = true;
                }

                public void SendPageComplateEvent(int pageId)
                {
                        Manage.PageManage.SendSuccess(pageId);
                }


                public void CloseClientCon()
                {
                        this._Client.Close();
                }

                public void CloseClientCon(int time)
                {
                        this._Client.Close(time);
                }

                public IPEndPoint GetClientAddress()
                {
                        return this._Client.GetClientAddress();
                }

        }
}
