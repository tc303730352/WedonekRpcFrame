using System.IO;

using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.HttpWebSocket
{
        internal class SocketResponse : ISocketResponse
        {
                private readonly IWebSocketClient _Client = null;
                public SocketResponse(IWebSocketClient client)
                {
                        this._Client = client;
                }
               
                public void Write(Stream stream)
                {
                        this._Client.Send(stream);
                }
                public void Write(string text)
                {
                        _Client.Send(text);
                }
        }
}
