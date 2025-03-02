using WeDonekRpc.Helper;
using WeDonekRpc.IOSendInterface;
using WeDonekRpc.TcpServer.Config;

namespace WeDonekRpc.TcpServer.Model
{
    /// <summary>
    /// 数据包
    /// </summary>
    internal struct DataPage
    {
        /// <summary>
        /// 数据包的类型
        /// </summary>
        public byte PageType;

        /// <summary>
        /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
        /// </summary>
        public byte DataType;


        /// <summary>
        /// 数据包的唯一标示
        /// </summary>
        public uint DataId;

        /// <summary>
        /// 数据的长度
        /// </summary>
        public int DataLen;

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompression;
        /// <summary>
        /// 头类型
        /// </summary>
        public byte HeadType;

        /// <summary>
        /// 包的类型(用户自定义)
        /// </summary>
        public char[] Type;

        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Content;


        private static byte[] _GetSendStream (Page page)
        {
            if (page.SendData == null)
            {
                return System.Array.Empty<byte>();
            }
            else if (page.SendType == ConstDicConfig.StreamSendType)
            {
                return (byte[])page.SendData;
            }
            else if (page.SendType == ConstDicConfig.StringSendType)
            {
                return ToolsHelper.SerializationString(page.SendData.ToString());
            }
            else
            {
                return ToolsHelper.SerializationClass(page.SendData);
            }
        }
        /// <summary>
        /// 获取数据包的信息
        /// </summary>
        /// <returns></returns>
        public static void InitDataPage (Page page, out DataPage data)
        {
            data = new DataPage
            {
                DataId = page.PageId
            };
            byte[] content = _GetSendStream(page);
            if (SocketConfig.IsCompression && content.LongLength >= Config.SocketConfig.CompressionSize)
            {
                data.IsCompression = true;
                byte[] myByte = ZipTools.Compression(content);
                data.DataLen = myByte.Length;
                data.Content = myByte;
            }
            else
            {
                data.Content = content;
                data.DataLen = content.Length;
            }
            data.DataType = page.SendType;
            data.PageType = page.PageType;
            data.Type = page.Type.ToCharArray();
        }
        public int GetPageSize ()
        {
            return this.Type.Length + ConstDicConfig.HeadLen + this.DataLen + 1;
        }
    }
}
