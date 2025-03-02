
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket
{
    internal class ApiService : IApiService
        {
                private readonly IWebSocketClient _Client;
                public ApiService (IWebSocketClient client, byte[] content)
                {
                        this._Client = client;
                        this.Request = new SocketRequest (client.Request, content);
                        this.Response = new SocketResponse (client);
                }
                public ApiService (IWebSocketClient client)
                {
                        this._Client = client;
                        this.Request = new SocketRequest (client.Request, null);
                        this.Response = new SocketResponse (client);
                }
                /// <summary>
                /// 会话状态
                /// </summary>
                public IClientSession Session => this._Client.Session;

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

                public void CloseCon (int time)
                {
                        this._Client.CloseClient (time);
                }

                public void CloseCon ()
                {
                        this._Client.CloseClient ();
                }
        }
}
