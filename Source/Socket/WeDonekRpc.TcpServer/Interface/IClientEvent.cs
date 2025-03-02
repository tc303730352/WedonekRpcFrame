using WeDonekRpc.TcpServer.Model;

namespace WeDonekRpc.TcpServer.Interface
{
    internal interface IClientEvent
    {
        string BindParam { get; }
        bool IsAuthorization { get; }
        /// <summary>
        /// 获取返回的数据包
        /// </summary>
        /// <param name="dataPage"></param>
        void AllotEvent (DataPageInfo dataPage);


        /// <summary>
        /// 发送数据包完成
        /// </summary>
        void SendPageComplateEvent (uint pageId);

        /// <summary>
        /// 发送包发生错误
        /// </summary>
        /// <param name="pageId"></param>
        void SendPageErrorEvent (uint pageId);


        /// <summary>
        /// 连接已关闭
        /// </summary>
        void ConCloseEvent ();
    }
}
