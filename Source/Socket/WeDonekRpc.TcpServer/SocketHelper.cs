using WeDonekRpc.Helper;
using WeDonekRpc.IOBuffer;
using WeDonekRpc.IOBuffer.Interface;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpServer.Enum;
using WeDonekRpc.TcpServer.Manage;
using WeDonekRpc.TcpServer.Model;

namespace WeDonekRpc.TcpServer
{
    internal class SocketHelper
    {
        /// <summary>
        /// 读取数据包
        /// </summary>
        /// <param name="arg"></param>
        internal static void ReadPage (GetDataPage arg)
        {
            if (arg.IsCompression)
            {
                arg.Content = ZipTools.Decompression(arg.Content);
            }
            arg.Allot.InitInfo(ref arg);
            object resData = arg.Allot.Action();
            if (( ConstDicConfig.SinglePagType & arg.PageType ) != ConstDicConfig.SinglePagType && resData != null)
            {
                ReplyPageManage.Send(arg, resData);
            }
        }
        /// <summary>
        /// 获取包头信息
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private static void _InitPageHead (ISocketBuffer buffer, ref DataPage page)
        {
            _ = buffer.WriteByte(ConstDicConfig.PageVer, 0);
            _ = buffer.WriteByte(page.PageType, 1);
            if (page.IsCompression)
            {
                page.DataType += 1;
            }
            _ = buffer.WriteByte(page.DataType, 2);
            _ = buffer.WriteByte((byte)page.Type.Length, 3);
            _ = buffer.WriteUInt(page.DataId, 4);
            _ = buffer.WriteInt(page.DataLen, 8);
            int index = ConstDicConfig.HeadLen;
            index += buffer.WriteChar(page.Type, index);
            if (page.DataLen != 0)
            {
                index += buffer.Write(page.Content, index);
            }
            _ = buffer.WriteByte(Tools.CSByByte(buffer.Stream, index), index);
        }
        internal static ISocketBuffer GetSendBuffer (ref DataPage page)
        {
            ISocketBuffer buffer = BufferCollect.ApplySendBuffer(page.GetPageSize(), uint.MinValue);
            _InitPageHead(buffer, ref page);
            return buffer;
        }
        internal static ISocketBuffer GetSendBuffer (ref DataPage page, uint pageId)
        {
            ISocketBuffer buffer = BufferCollect.ApplySendBuffer(page.GetPageSize(), pageId);
            _InitPageHead(buffer, ref page);
            return buffer;
        }
        /// <summary>
        /// 组建数据包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="objPage"></param>
        internal static bool SplitPage (byte[] data, ref int size, ref Model.DataPageInfo objPage, Client.SocketClient client)
        {
            int index = 0;
            return SocketHelper.SplitPage(data, ref size, ref index, ref objPage, client);
        }
        internal static bool SplitPage (byte[] data, ref int size, ref int index, ref Model.DataPageInfo objPage, Client.SocketClient client)
        {
            if (objPage == null)
            {
                if (data[index] != ConstDicConfig.PageVer)
                {
                    return false;
                }
                objPage = new Model.DataPageInfo();
            }
            if (objPage.LoadData(data, ref size, ref index))
            {
                client.ReceiveComplate();
                if (size != 0)
                {
                    return SocketHelper.SplitPage(data, ref size, ref index, ref objPage, client);
                }
                return true;
            }
            return objPage.LoadProgress != PageLoadProgress.包校验错误;
        }
        private static readonly int _v = 256;
        public static byte CS (byte[] one, byte[] two)
        {
            int num = 0;
            foreach (byte b in one)
            {
                num += b;
            }
            foreach (byte b in two)
            {
                num += b;
            }
            return (byte)( num % _v );
        }
    }
}
