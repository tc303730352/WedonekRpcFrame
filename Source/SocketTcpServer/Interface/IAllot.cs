using System;
using System.Net;
using System.Threading;

using SocketTcpServer.Enum;
using SocketTcpServer.Model;

using RpcHelper;

namespace SocketTcpServer.Interface
{
        public delegate void Async(IAsyncEvent e);
        public abstract class IAllot : ICloneable
        {
                private GetDataPage _Page = null;
                internal void InitInfo(GetDataPage page)
                {
                        this._Page = page;
                }
                protected int PageId => this._Page.PageId;
                protected IPEndPoint ClientIp => this._Page.Client.GetClientAddress();
                protected PageType PageType => this._Page.PageType;
                public string Type => this._Page.Type;

                /// <summary>
                /// 数据内容
                /// </summary>
                public byte[] Content => this._Page.PageContent;
                public Guid ServerId => this._Page.ServerId;


                protected Guid ClientId => this._Page.ClientId;

                /// <summary>
                /// 数据类型
                /// </summary>
                protected Enum.SendType DataType => this._Page.DataType;
                /// <summary>
                /// 客户端信息
                /// </summary>
                internal IClient ClientInfo => this._Page.Client;

                public ISocketClient Clinet => new ReplyClient(this._Page);
                /// <summary>
                /// 完成授权
                /// </summary>
                /// <param name="bindParam">绑定参数</param>
                internal void ComplateAuthorization(string bindParam)
                {
                        this.ClientInfo.AuthorizationComplate(bindParam);
                }
                private string _Str = null;
                /// <summary>
                /// 获取数据
                /// </summary>
                /// <returns></returns>
                protected string GetData()
                {
                        if (this._Str == null)
                        {
                                this._Str = SocketHelper.DeserializeStringData(this._Page.PageContent);
                        }
                        return this._Str;
                }

                protected T GetData<T>()
                {
                        string json = this.GetData();
                        if (json == string.Empty)
                        {
                                return default;
                        }
                        return Tools.Json<T>(json);
                }
                protected T GetValue<T>() where T : struct
                {
                        if (this.Content == null)
                        {
                                return default;
                        }
                        return (T)SocketHelper.GetValue(typeof(T), this._Page.PageContent);
                }
                public object Clone()
                {
                        return this.MemberwiseClone();
                }

                public virtual object Action()
                {
                        return null;
                }
                protected void Close()
                {
                        this._Page.Client.CloseClientCon();
                }
                protected void Close(int time)
                {
                        new Thread(new ThreadStart(delegate ()
                        {
                                Thread.Sleep(time);
                                this.ClientInfo.CloseClientCon();
                        })).Start();
                }
        }
}
