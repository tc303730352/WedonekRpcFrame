using System;
using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpServer.Model
{
    internal class Page
    {
        public static Page GetSysPage (string type, string str)
        {
            return new Page
            {
                PageType = ConstDicConfig.SystemPageType,
                SendData = str,
                SendType = ConstDicConfig.StringSendType,
                Type = type
            };
        }

        public static Page GetReply (ref GetDataPage arg, object data)
        {
            return new Page
            {
                SendData = data,
                SendType = ToolsHelper.GetSendType(data.GetType()),
                Type = arg.Type,
                PageType = ConstDicConfig.ReplyPageType,
                PageId = arg.PageId,
            };
        }
        public static Page GetReplyPage (ref GetDataPage arg, object data)
        {
            return new Page
            {
                SendData = data,
                SendType = ToolsHelper.GetSendType(data.GetType()),
                Type = arg.Type,
                PageType = arg.PageType == ConstDicConfig.SystemPageType ? arg.PageType : ConstDicConfig.ReplyPageType,
                PageId = arg.PageId,
            };
        }

        public Guid? ClientId;

        public Guid? ServerId;

        /// <summary>
        /// 父ID
        /// </summary>
        public uint PageId;

        public int TimeOut;

        public int SyncTimeOut;

        /// <summary>
        /// 数据包的类型
        /// </summary>
        public byte PageType;

        /// <summary>
        /// 需要发送的数据
        /// </summary>
        public object SendData;
        /// <summary>
        /// 类型
        /// </summary>
        public string Type;
        /// <summary>
        /// 发送的类型
        /// </summary>
        public byte SendType;

    }
}
