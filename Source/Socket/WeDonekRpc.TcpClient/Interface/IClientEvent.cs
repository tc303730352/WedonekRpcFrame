using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.Interface
{
        internal interface IClientEvent
        {
                /// <summary>
                /// 获取返回的数据包
                /// </summary>
                /// <param name="dataPage"></param>
                void AllotEvent(DataPageInfo dataPage);


                /// <summary>
                /// 发送包发生错误
                /// </summary>
                /// <param name="error"></param>
                void SendPageErrorEvent(uint pageId);

                /// <summary>
                /// 连接完成
                /// </summary>
                void ConnectComplateEvent();

                /// <summary>
                /// 服务端授权完成
                /// </summary>
                void AuthorizationComplate();

                /// <summary>
                /// 发送数据包完成
                /// </summary>
                void SendPageComplateEvent(uint pageId);

                /// <summary>
                /// 连接已关闭
                /// </summary>
                void ConCloseEvent();

                /// <summary>
                /// 链接失败
                /// </summary>
                void ConError();
        }
}
