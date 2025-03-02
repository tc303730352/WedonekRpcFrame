using System;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Config;
namespace WeDonekRpc.TcpClient.Model
{
    internal class Page : ICloneable
    {
        private static readonly byte _SingleDataPage = (byte)( ConstDicConfig.SinglePagType + ConstDicConfig.DataPageType );

        private static readonly byte _SingleFilePage = (byte)( ConstDicConfig.SinglePagType + ConstDicConfig.FilePageType );

        private static uint _PageId = 1;

        private static uint _GetPageId ()
        {
            return Interlocked.Increment(ref _PageId);
        }
        public static Page GetReplyPage (GetDataPage page, string type, object data)
        {
            return new Page
            {
                PageId = page.PageId,
                PageType = page.PageType == ConstDicConfig.SystemPageType ? page.PageType : ConstDicConfig.ReplyPageType,
                SendData = data,
                SendType = ToolsHelper.GetSendType(data.GetType()),
                Type = type
            };
        }
        public static Page GetSysPage (string type, string str)
        {
            return new Page
            {
                PageType = ConstDicConfig.SystemPageType,
                SendType = ConstDicConfig.StringSendType,
                PageId = 0,
                SendData = str,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetPingPage (string serverId)
        {
            return new Page
            {
                ServerId = serverId,
                PageId = _GetPageId(),
                PageType = ConstDicConfig.PingPageType,
                SendType = ConstDicConfig.StringSendType,
                SendData = null,
                Type = "ping",
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetDataPage (string serverId, string type, string str)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = ConstDicConfig.DataPageType,
                PageId = _GetPageId(),
                SendData = str,
                SendType = ConstDicConfig.StringSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetDataPage (string serverId, string type)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = ConstDicConfig.DataPageType,
                PageId = _GetPageId(),
                SendType = ConstDicConfig.StringSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetDataPage<T> (string serverId, string type, T data)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = ConstDicConfig.DataPageType,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.ObjectSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetDataPage<T> (string serverId, string type, T data, int? timeOut)
        {
            Page page = new Page
            {
                ServerId = serverId,
                PageType = ConstDicConfig.DataPageType,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.ObjectSendType,
                Type = type
            };
            if (timeOut.HasValue)
            {
                page.TimeOut = timeOut.Value;
                page.SyncTimeOut = timeOut.Value * 1150;
            }
            else
            {
                page.TimeOut = SocketConfig.TimeOut;
                page.SyncTimeOut = SocketConfig.SyncTimeOut;
            }
            return page;
        }
        public static Page GetSingleDataPage (string serverId, string type, string str)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = _SingleDataPage,
                PageId = _GetPageId(),
                SendData = str,
                SendType = ConstDicConfig.StringSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetSingleDataPage (string serverId, string type)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = _SingleDataPage,
                PageId = _GetPageId(),
                SendType = ConstDicConfig.StringSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetSingleDataPage<T> (string serverId, string type, T data)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = _SingleDataPage,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.ObjectSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetSingleDataPage<T> (string serverId, string type, T data, int? timeout)
        {
            Page page = new Page
            {
                ServerId = serverId,
                PageType = _SingleDataPage,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.ObjectSendType,
                Type = type
            };
            if (timeout.HasValue)
            {
                page.TimeOut = timeout.Value;
                page.SyncTimeOut = timeout.Value * 1150;
            }
            else
            {
                page.TimeOut = SocketConfig.TimeOut;
                page.SyncTimeOut = SocketConfig.SyncTimeOut;
            }
            return page;
        }
        public static Page GetUpFilePage<T> (string serverId, string type, T data)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = ConstDicConfig.FilePageType,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.ObjectSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetUpFilePage (string serverId, string type, byte[] data)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = ConstDicConfig.FilePageType,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.StreamSendType,
                Type = type,
                TimeOut = SocketConfig.TimeOut,
                SyncTimeOut = SocketConfig.SyncTimeOut
            };
        }
        public static Page GetUpFileStream (string serverId, byte[] data)
        {
            return new Page
            {
                ServerId = serverId,
                PageType = _SingleFilePage,
                PageId = _GetPageId(),
                SendData = data,
                SendType = ConstDicConfig.StreamSendType,
                Type = "WriteStream",
                TimeOut = SocketConfig.TimeOut
            };
        }

        public uint PageId;
        public int TimeOut;
        public int SyncTimeOut;
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerId;


        /// <summary>
        /// 需要发送的数据
        /// </summary>
        public object SendData;
        /// <summary>
        /// 类型
        /// </summary>
        public string Type;

        /// <summary>
        /// 数据包的类型
        /// </summary>
        public byte PageType;

        /// <summary>
        /// 发送的类型
        /// </summary>
        public byte SendType;
        private static byte[] _GetSendStream (Page page)
        {
            if (page.SendData == null)
            {
                return Array.Empty<byte>();
            }
            else if (page.SendType == ConstDicConfig.ObjectSendType)
            {
                return ToolsHelper.SerializationClass(page.SendData);
            }
            else if (page.SendType == ConstDicConfig.StreamSendType)
            {
                return (byte[])page.SendData;
            }
            else
            {
                return ToolsHelper.SerializationString((string)page.SendData);
            }
        }
        /// <summary>
        /// 获取数据包的信息
        /// </summary>
        /// <returns></returns>
        public static DataPage GetDataPage (Page page)
        {
            DataPage temp = new DataPage
            {
                DataId = page.PageId,
                SendTime = HeartbeatTimeHelper.HeartbeatTime,
                ConTimeOut = HeartbeatTimeHelper.HeartbeatTime + Config.SocketConfig.ConTimeout,
                DataType = page.SendType,
                PageType = page.PageType,
                Type = page.Type.ToCharArray()
            };
            byte[] content = _GetSendStream(page);
            if (SocketConfig.IsCompression && content.LongLength >= Config.SocketConfig.CompressionSize)
            {
                temp.IsCompression = true;
                byte[] myByte = ZipTools.Compression(content);
                temp.DataLen = myByte.Length;
                temp.Content = myByte;
            }
            else
            {
                temp.Content = content;
                temp.DataLen = content.Length;
            }
            temp.TypeLen = (byte)temp.Type.Length;
            return temp;
        }
        public object Clone ()
        {
            return this.MemberwiseClone();
        }
    }
}
