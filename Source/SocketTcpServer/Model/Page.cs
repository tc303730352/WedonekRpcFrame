using System;
using System.Threading;

using SocketTcpServer.Enum;

namespace SocketTcpServer.Model
{
        internal class Page : ICloneable
        {
                private static readonly PageType _SingleDataPage = PageType.数据包 | PageType.单向;
                private static int _PageId = 1;

                private static int _GetPageId()
                {
                        return Interlocked.Increment(ref _PageId);
                }

                public static Page GetSysPage(string type, string str)
                {
                        return new Page
                        {
                                PageType = PageType.系统包,
                                SendData = str,
                                SendType = SendType.字符串,
                                Type = type
                        };
                }


                public static Page GetDataPage(Guid serverId, Guid clientId, string type, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                ClientId = clientId,
                                PageType = PageType.数据包,
                                PageId = _GetPageId(),
                                SendData = new byte[0],
                                SendType = SendType.字节流,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetDataPage<T>(Guid serverId, Guid clientId, string type, T data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                ClientId = clientId,
                                PageType = PageType.数据包,
                                PageId = _GetPageId(),
                                SendData = data,
                                SendType = SendType.对象,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }

                public static Page GetSingleDataPage(Guid serverId, Guid clientId, string type, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                ClientId = clientId,
                                PageType = _SingleDataPage,
                                PageId = -1,
                                SendData = new byte[0],
                                SendType = SendType.字节流,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetSingleDataPage<T>(Guid serverId, Guid clientId, string type, T data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ClientId = clientId,
                                ServerId = serverId,
                                PageType = _SingleDataPage,
                                PageId = -1,
                                SendData = data,
                                SendType = SendType.对象,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetBatchDataPage<T>(string type, T data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                PageType = _SingleDataPage,
                                SendData = data,
                                PageId = -1,
                                SendType = SendType.对象,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetReply(GetDataPage arg, object data)
                {
                        return new Page
                        {
                                SendData = data,
                                SendType = SocketHelper.GetSendType(data.GetType()),
                                Type = arg.Type,
                                PageType = PageType.回复包,
                                PageId = arg.PageId,
                        };
                }
                public static Page GetReplyPage(GetDataPage arg, object data)
                {
                        return new Page
                        {
                                SendData = data,
                                SendType = SocketHelper.GetSendType(data.GetType()),
                                Type = arg.Type,
                                PageType = arg.PageType == PageType.系统包 ? arg.PageType : PageType.回复包,
                                PageId = arg.PageId,
                        };
                }

                public Guid ClientId
                {
                        get;
                        private set;
                }
                public Guid ServerId { get; private set; }
                /// <summary>
                /// 父ID
                /// </summary>
                internal int PageId { get; private set; }
                /// <summary>
                /// 需要发送的数据
                /// </summary>
                internal object SendData
                {
                        get;
                        private set;
                }
                /// <summary>
                /// 类型
                /// </summary>
                internal string Type
                {
                        get;
                        private set;
                }

                /// <summary>
                /// 数据包的类型
                /// </summary>
                internal Enum.PageType PageType
                {
                        get;
                        private set;
                }

                /// <summary>
                /// 发送的类型
                /// </summary>
                internal Enum.SendType SendType
                {
                        get;
                        private set;
                }
                public int TimeOut
                {
                        get;
                        private set;
                }
                public int SyncTimeOut
                {
                        get;
                        private set;
                }

                public object Clone()
                {
                        return this.MemberwiseClone();
                }
        }
}
