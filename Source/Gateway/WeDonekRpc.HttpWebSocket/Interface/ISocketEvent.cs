
using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    public interface ISocketEvent
        {
                /// <summary>
                /// 授权
                /// </summary>
                /// <param name="head"></param>
                /// <returns></returns>
                bool Authorize(RequestBody head);
                /// <summary>
                /// 检查会话
                /// </summary>
                /// <param name="session"></param>
                /// <param name="error"></param>
                /// <returns></returns>
                void CheckSession(IClientSession session);

                /// <summary>
                /// 会话离线
                /// </summary>
                /// <param name="session"></param>
                void SessionOffline(IClientSession session);
                /// <summary>
                /// 授权完成
                /// </summary>
                /// <param name="service"></param>
                void AuthorizeComplate(IApiService service);
                /// <summary>
                /// 收到的信息
                /// </summary>
                /// <param name="service"></param>
                void Receive(IApiService service);
                /// <summary>
                /// 回复错误
                /// </summary>
                /// <param name="service"></param>
                /// <param name="e"></param>
                /// <param name="source">来源</param>
                void ReplyError(IApiService service, ErrorException e, string source);
        }

}
