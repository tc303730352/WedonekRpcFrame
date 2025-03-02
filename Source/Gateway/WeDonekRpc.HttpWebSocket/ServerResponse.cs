
using System;
using System.IO;

using WeDonekRpc.HttpWebSocket.Interface;

using WeDonekRpc.Helper;

namespace WeDonekRpc.HttpWebSocket
{
        internal class ServerResponse : IServerResponse
        {
                private readonly IWebSocketConfig _Config = null;
                private readonly IClient _Client = null;
                public ServerResponse(IWebSocketConfig config, IClient client)
                {
                        this._Client = client;
                        this._Config = config;
                }


                public void Send(Stream stream)
                {
                      
                }
                public void Send(string text)
                {
                        this._Client.Send(text);
                }
           
        }
}
