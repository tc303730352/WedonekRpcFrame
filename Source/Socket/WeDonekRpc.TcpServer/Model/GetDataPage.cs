namespace WeDonekRpc.TcpServer.Model
{
    internal struct GetDataPage
    {
        /// <summary>
        /// 包的ID
        /// </summary>
        public uint PageId;

        /// <summary>
        /// 数据类型（1 字符串类型 , 2 实体对象 3 字节流）
        /// </summary>

        public byte DataType;

        /// <summary>
        /// 包类型
        /// </summary>
        public byte PageType;

        /// <summary>
        /// 是否压缩
        /// </summary>
        public bool IsCompression;

        /// <summary>
        /// 数据内容
        /// </summary>
        public byte[] Content;

        /// <summary>
        /// 用于处理包的处理接口
        /// </summary>
        public Interface.IAllot Allot;
        /// <summary>
        /// 客户端信息
        /// </summary>
        public Interface.IClient Client;

        /// <summary>
        /// 包的类型(用户自定义)
        /// </summary>
        public string Type;
    }
}
