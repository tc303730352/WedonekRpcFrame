using System.IO;

namespace HttpWebSocket.Interface
{
        internal interface IServerResponse
        {
                void Send(Stream stream);

                void Send(string text);
        }
}