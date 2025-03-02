using WeDonekRpc.IOSendInterface;

namespace WeDonekRpc.TcpClient.Model
{
    /// <summary>
    /// 数据包
    /// </summary>
    internal class DataPage
    {
        /// <summary>
        /// 链接超时时间
        /// </summary>
        public int ConTimeOut;

        public int SendTime;

        /// <summary>
        /// 数据的长度
        /// </summary>
        public int DataLen;

        /// <summary>
        /// 数据包的类型
        /// </summary>
        public byte PageType;


        /// <summary>
        /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
        /// </summary>
        public byte DataType;

        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Content;

        /// <summary>
        /// 数据包的唯一标示
        /// </summary>
        public uint DataId;

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompression;
        /// <summary>
        /// 包的类型(用户自定义)
        /// </summary>
        public char[] Type;

        public byte TypeLen;

        public int GetPageSize ()
        {
            return this.TypeLen + ConstDicConfig.HeadLen + this.DataLen + 1;
        }
    }
}
