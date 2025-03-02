using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Log;
using WeDonekRpc.IOBuffer;
using WeDonekRpc.IOBuffer.Interface;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpClient.Client;
using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient
{
    internal class SocketTools
    {
        public static void ReadPage ( GetDataPage arg )
        {
            if ( arg.IsCompression )
            {
                arg.Content = ZipTools.Decompression(arg.Content);
            }
            arg.Allot.InitInfo(arg);
            string type = arg.Type;
            object resData = arg.Allot.Action(ref type);
            if ( resData != null && ( ConstDicConfig.SinglePagType & arg.PageType ) != ConstDicConfig.SinglePagType )
            {
                Page page = Page.GetReplyPage(arg, type, resData);
                if ( !arg.Client.SendPage(page, out string error) )
                {
                    new LogInfo(error, LogGrade.ERROR, "Socket_Client")
                    {
                        LogTitle = "回复事件包错误!",
                        LogContent = page.ToJson()
                    }.Save();
                }
            }
        }
        /// <summary>
        /// 组建数据包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <param name="objPage"></param>
        internal static bool SplitPage ( byte[] data, ref int len, ref Model.DataPageInfo objPage, SocketClient client )
        {
            int index = 0;
            return SocketTools.SplitPage(data, ref len, ref index, ref objPage, client);
        }
        internal static bool SplitPage ( byte[] data, ref int len, ref int index, ref Model.DataPageInfo objPage, SocketClient client )
        {
            if ( objPage == null )
            {
                if ( data[index] != ConstDicConfig.PageVer )
                {
                    return false;
                }
                objPage = new Model.DataPageInfo();
            }
            if ( objPage.LoadData(data, ref len, ref index) )
            {
                client.ReceiveComplate();
                if ( len != 0 )
                {
                    return SocketTools.SplitPage(data, ref len, ref index, ref objPage, client);
                }
                return true;
            }
            return objPage.LoadProgress != PageLoadProgress.包校验错误;
        }

        private static readonly int _v = 256;
        public static byte CS ( byte[] one, byte[] content )
        {
            long num = 0;
            foreach ( byte b in one )
            {
                num += b;
            }
            foreach ( byte b in content )
            {
                num += b;
            }
            return (byte)( num % _v );
        }

        /// <summary>
        /// 获取发送的字节流
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        internal static ISocketBuffer GetSendBuffer ( DataPage page )
        {
            ISocketBuffer buffer = BufferCollect.ApplySendBuffer(page.GetPageSize(), page.DataId);
            _InitPageHead(buffer, page);
            return buffer;
        }


        /// <summary>
        /// 获取包头信息
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        private static void _InitPageHead ( ISocketBuffer buffer, DataPage page )
        {
            _ = buffer.WriteByte(ConstDicConfig.PageVer, 0);
            _ = buffer.WriteByte(page.PageType, 1);
            if ( page.IsCompression )
            {
                page.DataType += 1;
            }
            _ = buffer.WriteByte(page.DataType, 2);
            _ = buffer.WriteByte(page.TypeLen, 3);
            _ = buffer.WriteUInt(page.DataId, 4);
            _ = buffer.WriteInt(page.DataLen, 8);
            int index = ConstDicConfig.HeadLen;
            index += buffer.WriteChar(page.Type, index);
            if ( page.DataLen != 0 )
            {
                index += buffer.Write(page.Content, index);
            }
            _ = buffer.WriteByte(Tools.CSByByte(buffer.Stream, index), index);
        }
    }
}
