using WeDonekRpc.TcpServer.Interface;

namespace WeDonekRpc.TcpServer.Model
{
    internal class ReplyPage
    {
        /// <summary>
        /// 客户端信息
        /// </summary>
        public IClient client;
        public Page page;
        public uint id;
        public int retryNum = 0;
    }
}
