using WeDonekRpc.Helper;
using WeDonekRpc.TcpClient.Enum;

namespace WeDonekRpc.TcpClient.Model
{
    internal class PageDetailed
    {
        public PageDetailed (Model.SyntonySet returnSet, Page page)
        {
            this.Status = PageStatus.等待发送;
            this.SyntonySet = returnSet;
            this.PageType = page.PageType;
            this.TimeOut = HeartbeatTimeHelper.HeartbeatTime + page.TimeOut;
        }
        public byte PageType;
        internal byte[] ReturnData;
        public byte DataType;
        public bool IsError;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error;

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public int TimeOut;


        /// <summary>
        /// 包的状态
        /// </summary>
        internal volatile PageStatus Status;

        /// <summary>
        /// 回调设置
        /// </summary>
        internal SyntonySet SyntonySet;
    }
}
