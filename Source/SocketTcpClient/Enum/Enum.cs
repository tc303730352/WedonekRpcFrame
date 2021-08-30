
namespace SocketTcpClient.Enum
{
        /// <summary>
        /// 发送类型
        /// </summary>
        public enum SendType
        {
                字符串 = 0,
                对象 = 16,
                字节流 = 32
        }

        /// <summary>
        /// 包的状态
        /// </summary>
        internal enum PageStatus
        {
                等待发送 = 2,
                已发送 = 4,
                回复完成 = 32
        }

        /// <summary>
        /// 回凋类型
        /// </summary>
        internal enum SyntonyType
        {
                同步,
                异步
        }

        /// <summary>
        /// 客户端状态
        /// </summary>
        internal enum ClientStatus
        {
                未连接 = 0,
                正在连接 = 1,
                等待发送 = 2,
                链接成功 = 3,
                以关闭 = 4
        }

        /// <summary>
        /// 数据包的类型
        /// </summary>
        internal enum PageType
        {
                系统包 = 2,
                数据包 = 4,
                单向 = 8,
                回复包 = 16,
                文件上传 = 32,
                系统数据包 = 64,
                ping包 = 1
        }

        /// <summary>
        /// 包加载的进度
        /// </summary>
        internal enum PageLoadProgress
        {
                加载中 = 0,
                头部加载完成 = 1,
                加载完成 = 2,
                包体加载完成 = 3,
                包校验错误 = 4
        }

}
