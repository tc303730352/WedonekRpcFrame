using System;

using SocketTcpServer.Enum;

namespace SocketTcpServer.Model
{
        internal class PageInfo
        {
                internal PageInfo(DataPage page, Guid serveId, Guid clientId)
                {
                        this.LastTime = DateTime.Now;
                        this.PageData = page;
                        this.ServerId = serveId;
                        this.ClientId = clientId;
                }

                /// <summary>
                /// 服务端ID
                /// </summary>
                internal Guid ServerId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 客户端ID
                /// </summary>
                internal Guid ClientId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 数据包内容
                /// </summary>
                internal DataPage PageData
                {
                        get;
                        set;
                }

                private PageStatus _Status = PageStatus.发送中;

                /// <summary>
                /// 包的状态
                /// </summary>
                internal PageStatus Status
                {
                        get => this._Status;
                        set
                        {
                                this._Status = value;
                                this.LastTime = DateTime.Now;
                                if (this._Status == PageStatus.发送错误)
                                {
                                        ++this.SendErrorNum;
                                }
                        }
                }

                /// <summary>
                /// 发送的错误数
                /// </summary>
                internal int SendErrorNum
                {
                        get;
                        set;
                }

                /// <summary>
                /// 最后操作时间
                /// </summary>
                internal DateTime LastTime
                {
                        get;
                        set;
                }
        }
}
