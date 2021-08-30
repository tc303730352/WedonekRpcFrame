
using HttpWebSocket.Interface;
using HttpWebSocket.Model;

namespace HttpWebSocket
{
        internal class ApiService : IApiService
        {
                public ApiService(IWebSocketClient client, byte[] content)
                {
                        this.Session = client.Session;
                        this.Request = new SocketRequest(client.Request, content);
                        this.Response = new SocketResponse(client);
                }
                public ApiService(IWebSocketClient client)
                {
                        this.Session = client.Session;
                        this.Request = new SocketRequest(client.Request, null);
                        this.Response = new SocketResponse(client);
                }
                /// <summary>
                /// 会话状态
                /// </summary>
                public IClientSession Session { get; }

                /// <summary>
                /// 请求
                /// </summary>
                public ISocketRequest Request
                {
                        get;
                }
                /// <summary>
                /// 响应
                /// </summary>
                public ISocketResponse Response
                {
                        get;
                }

                public void CloseCon(int time)
                {
                        throw new System.NotImplementedException();
                }

                public void CloseCon()
                {
                        throw new System.NotImplementedException();
                }
        }
}
