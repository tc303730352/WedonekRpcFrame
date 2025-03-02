namespace WeDonekRpc.TcpClient.Model
{
    internal class GetDataPage
    {
        /// <summary>
        /// 包的ID
        /// </summary>
        internal uint PageId;

        /// <summary>
        /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
        /// </summary>
        public byte DataType;
        /// <summary>
        /// 包的类型(用户自定义)
        /// </summary>
        public string Type;

        public bool IsCompression;

        /// <summary>
        /// 数据内容
        /// </summary>
        internal byte[] Content;

        /// <summary>
        /// 用于处理包的处理接口
        /// </summary>
        internal Interface.IAllot Allot;


        /// <summary>
        /// 客户端信息
        /// </summary>
        internal Interface.IClient Client;

        public byte PageType;
    }
}
