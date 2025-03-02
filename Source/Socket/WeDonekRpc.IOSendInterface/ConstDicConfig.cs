namespace WeDonekRpc.IOSendInterface
{
    public static class ConstDicConfig
    {
        /// <summary>
        /// 包版本号
        /// </summary>
        public const byte PageVer = 253;
        /// <summary>
        /// 包头长度
        /// </summary>
        public const int HeadLen = 12;

        /// <summary>
        /// 字符串发送类型
        /// </summary>
        public const byte StringSendType = 2;

        /// <summary>
        /// 字节流发送类型
        /// </summary>
        public const byte StreamSendType = 8;

        /// <summary>
        /// 对象的发送类型
        /// </summary>
        public const byte ObjectSendType = 4;


        /// <summary>
        /// 是否压缩
        /// </summary>
        public const byte IntLen = 5;

        /// <summary>
        /// 单向包类型
        /// </summary>
        public const byte SinglePagType = 8;
        /// <summary>
        /// 系统包类型
        /// </summary>
        public const byte SystemPageType = 2;

        /// <summary>
        /// ping包
        /// </summary>
        public const byte PingPageType = 1;
        /// <summary>
        /// 回复包类型
        /// </summary>
        public const byte ReplyPageType = 16;
        /// <summary>
        /// 文件包类型
        /// </summary>
        public const byte FilePageType = 32;

        /// <summary>
        /// 数据包类型
        /// </summary>
        public const byte DataPageType = 4;
    }
}
