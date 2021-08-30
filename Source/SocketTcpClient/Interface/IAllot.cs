using System;

using SocketTcpClient.Model;

namespace SocketTcpClient.Interface
{
        public abstract class IAllot : ICloneable
        {
                internal void InitInfo(GetDataPage page)
                {
                        this.PageId = page.PageId;
                        this.ClientInfo = page.Client;
                        this.DataType = page.DataType;
                        this.Content = page.PageContent;
                        this.AddTime = DateTime.Now;
                }

                /// <summary>
                /// 数据内容
                /// </summary>
                internal byte[] Content
                {
                        get;
                        private set;
                }
                protected internal int PageId
                {
                        get;
                        private set;
                }

                /// <summary>
                /// 接收时间
                /// </summary>
                public DateTime AddTime
                {
                        get;
                        private set;
                }

                /// <summary>
                /// 数据类型
                /// </summary>
                protected Enum.SendType DataType
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 客户端信息
                /// </summary>
                internal IClient ClientInfo
                {
                        get;
                        private set;
                }

                /// <summary>
                /// 完成授权
                /// </summary>
                internal void ComplateAuthorization()
                {
                        if (this.ClientInfo != null)
                        {
                                this.ClientInfo.AuthorizationComplate();
                        }
                }

                /// <summary>
                /// 获取数据
                /// </summary>
                /// <returns></returns>
                public string GetData()
                {
                        return this.DataType == Enum.SendType.字符串 ? SocketTools.DeserializeStringData(this.Content) : null;
                }

                public T GetData<T>()
                {
                        return SocketTools.DeserializeData<T>(this.Content);
                }

                public object Clone()
                {
                        return this.MemberwiseClone();
                }

                internal virtual object Action(ref string type)
                {
                        return this.ActionInfo(type);
                }
                public virtual object ActionInfo(string type)
                {
                        return null;
                }
                protected void Close()
                {
                        if (this.ClientInfo != null)
                        {
                                this.ClientInfo.CloseClientCon();
                        }
                }
                protected void Close(int time)
                {
                        if (this.ClientInfo != null)
                        {
                                this.ClientInfo.CloseClientCon(time);
                        }
                }
        }
}
