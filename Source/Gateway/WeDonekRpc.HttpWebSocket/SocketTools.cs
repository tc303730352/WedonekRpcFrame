using System;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.HttpWebSocket
{
    internal class SocketTools
    {
        private static readonly byte[] _Ping = new byte[] { 137, 0 };
        private static readonly byte[] _Pong = new byte[] { 138, 0 };
        private static readonly byte[] _Close = new byte[] { 136, 0 };

        public static byte[] Ping => _Ping;

        public static byte[] Pong => _Pong;

        public static byte[] Close => _Close;

        public static bool AnalysisWebPage (byte[] myByte, ref HttpHead head, int len)
        {
            if (head == null)
            {
                head = new HttpHead(myByte, len);
                if (!head.CheckHead())
                {
                    return false;
                }
            }
            else if (!head.AppendStream(myByte, len))
            {
                return false;
            }
            if (head.CheckIsEnd())
            {
                head.LoadPage();
            }
            return true;
        }

        internal static void AnalysisPage (byte[] myByte, int len, ref DataPageInfo page, lSocketClient client)
        {
            int index = 0;
            _AnalysisPage(myByte, len, ref index, ref page, client);
        }

        public static byte[] GetResponsePage (byte[] datas, PageType type)
        {
            byte[] stream = new byte[2];
            stream[0] = (byte)( (byte)type + 128 );
            byte[] len = null;
            if (datas.Length > ushort.MaxValue)
            {
                stream[1] = 127;
                len = BitConverter.GetBytes(datas.LongLength);
                Array.Reverse(len);
            }
            else if (datas.Length > 125)
            {
                stream[1] = 126;
                len = BitConverter.GetBytes((ushort)datas.Length);
                Array.Reverse(len);
            }
            else
            {
                stream[1] = (byte)datas.Length;
            }
            return stream.Add(len, datas);
        }
        private static void _AnalysisPage (byte[] myByte, int len, ref int index, ref DataPageInfo page, lSocketClient client)
        {
            if (page == null)
            {
                if (myByte[index] != 128 && myByte[index] != 129)
                {
                    return;
                }
                page = new DataPageInfo();
            }
            if (page.LoadData(myByte, len, ref index))
            {
                page.InitPage();
                client.ReceiveComplate();
                if (index != len)
                {
                    SocketTools._AnalysisPage(myByte, len, ref index, ref page, client);
                }
            }
        }

    }
}
