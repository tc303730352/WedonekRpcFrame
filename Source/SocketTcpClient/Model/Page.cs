using System;
using System.Threading;

using SocketTcpClient.Config;
using SocketTcpClient.Enum;

using RpcHelper;

namespace SocketTcpClient.Model
{
        internal class Page : ICloneable
        {
                private static readonly PageType _SingleDataPage = PageType.数据包 | PageType.单向;
                private static readonly PageType _SingleFilePage = PageType.文件上传 | PageType.单向;
                private static int _PageId = 1;

                private static int _GetPageId()
                {
                        return Interlocked.Increment(ref _PageId);
                }
                public static Page GetReplyPage(GetDataPage page, string type, object data)
                {
                        return new Page
                        {
                                PageId = page.PageId,
                                PageType = page.PageType == PageType.系统包 ? page.PageType : PageType.回复包,
                                SendData = data,
                                SendType = SocketTools.GetSendType(data),
                                Type = type,
                                TimeOut = SocketConfig.Timeout,
                                SyncTimeOut = SocketConfig.SyncTimeOut
                        };
                }
                public static Page GetSysPage(string type, string str)
                {
                        return new Page
                        {
                                PageType = PageType.系统包,
                                SendType = SendType.字符串,
                                PageId = -1,
                                SendData = str,
                                Type = type
                        };
                }
                public static Page GetPingPage(string serverId)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageId = _GetPageId(),
                                PageType = PageType.ping包,
                                SendType = SendType.字符串,
                                SendData = null,
                                Type = "ping",
                                TimeOut = SocketConfig.Timeout,
                                SyncTimeOut = SocketConfig.SyncTimeOut
                        };
                }
                public static Page GetDataPage(string serverId, string type, string str, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = PageType.数据包,
                                PageId = _GetPageId(),
                                SendData = str,
                                SendType = SendType.字符串,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetDataPage(string serverId, string type, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = PageType.数据包,
                                PageId = _GetPageId(),
                                SendType = SendType.字符串,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetDataPage<T>(string serverId, string type, T data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = PageType.数据包,
                                PageId = _GetPageId(),
                                SendData = data,
                                SendType = SendType.对象,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetSingleDataPage(string serverId, string type, string str, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = _SingleDataPage,
                                PageId = _GetPageId(),
                                SendData = str,
                                SendType = SendType.字符串,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetSingleDataPage(string serverId, string type, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = _SingleDataPage,
                                PageId = _GetPageId(),
                                SendType = SendType.字符串,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetSingleDataPage<T>(string serverId, string type, T data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = _SingleDataPage,
                                PageId = _GetPageId(),
                                SendData = data,
                                SendType = SendType.对象,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetUpFilePage<T>(string serverId, string type, T data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = PageType.文件上传,
                                PageId = _GetPageId(),
                                SendData = data,
                                SendType = SendType.对象,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetUpFilePage(string serverId, string type, byte[] data, int timeOut, int syncTimeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = PageType.文件上传,
                                PageId = _GetPageId(),
                                SendData = data,
                                SendType = SendType.字节流,
                                Type = type,
                                TimeOut = timeOut,
                                SyncTimeOut = syncTimeOut
                        };
                }
                public static Page GetUpFileStream(string serverId, byte[] data, int timeOut)
                {
                        return new Page
                        {
                                ServerId = serverId,
                                PageType = _SingleFilePage,
                                PageId = _GetPageId(),
                                SendData = data,
                                SendType = SendType.字节流,
                                Type = "WriteStream",
                                TimeOut = timeOut
                        };
                }

                public int PageId
                {
                        get;
                        private set;
                }
                public int TimeOut
                {
                        get;
                        set;
                }
                public int SyncTimeOut
                {
                        get;
                        set;
                }
                /// <summary>
                /// 服务器IP
                /// </summary>
                public string ServerId
                {
                        get;
                        set;
                }


                /// <summary>
                /// 需要发送的数据
                /// </summary>
                public object SendData
                {
                        get;
                        set;
                }
                /// <summary>
                /// 类型
                /// </summary>
                public string Type
                {
                        get;
                        set;
                }

                /// <summary>
                /// 数据包的类型
                /// </summary>
                public Enum.PageType PageType
                {
                        get;
                        set;
                }

                /// <summary>
                /// 发送的类型
                /// </summary>
                public Enum.SendType SendType
                {
                        get;
                        set;
                }
                private static byte[] _GetSendStream(Page page)
                {
                        if (page.SendData == null)
                        {
                                return new byte[0];
                        }
                        else if (page.SendType == SendType.对象)
                        {
                                return SocketTools.SerializationClass(page.SendData);
                        }
                        else if (page.SendType == SendType.字节流)
                        {
                                return (byte[])page.SendData;
                        }
                        else
                        {
                                return SocketTools.SerializationString((string)page.SendData);
                        }
                }
                /// <summary>
                /// 获取数据包的信息
                /// </summary>
                /// <returns></returns>
                public static DataPage GetDataPage(Page page)
                {
                        DataPage temp = new DataPage
                        {
                                DataId = page.PageId,
                                ConTimeOut = HeartbeatTimeHelper.HeartbeatTime + Config.SocketConfig.ConTimeout
                        };
                        byte[] content = _GetSendStream(page);
                        if (SocketConfig.IsCompression && content.LongLength >= Config.SocketConfig.CompressionSize)
                        {
                                temp.IsCompression = true;
                                temp.OriginalSize = content.Length;
                                byte[] myByte = Tools.CompressionData(content);
                                temp.DataLen = myByte.Length;
                                temp.Content = myByte;
                        }
                        else
                        {
                                temp.OriginalSize = content.Length;
                                temp.Content = content;
                                temp.DataLen = content.Length;
                        }
                        temp.DataType = page.SendType;
                        temp.PageType = page.PageType;
                        char[] chars = page.Type.ToCharArray();
                        temp.Type = chars;
                        temp.TypeLen = (byte)page.Type.Length;
                        return temp;
                }
                public object Clone()
                {
                        return this.MemberwiseClone();
                }
        }
}
