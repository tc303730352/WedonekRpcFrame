using System.IO;

namespace WeDonekRpc.HttpWebSocket.Interface
{
        internal interface IServerResponse
        {
                void Send(Stream stream);

                void Send(string text);
        }
}