using SocketTcpServer.Model;

namespace SocketTcpServer.Interface
{
        internal interface IClientEvent
        {
                /// <summary>
                /// 获取返回的数据包
                /// </summary>
                /// <param name="dataPage"></param>
                void AllotEvent(DataPageInfo dataPage);


                /// <summary>
                /// 发送数据包完成
                /// </summary>
                void SendPageComplateEvent(int pageId);

                /// <summary>
                /// 发送包发生错误
                /// </summary>
                /// <param name="error"></param>
                void SendPageErrorEvent(int pageId, string error);


                /// <summary>
                /// 连接已关闭
                /// </summary>
                void ConCloseEvent();
        }
}
