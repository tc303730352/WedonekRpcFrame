using System.IO;

namespace WeDonekRpc.HttpWebSocket.Interface
{
        public interface ISocketResponse
        {
                /// <summary>
                /// 写入
                /// </summary>
                /// <param name="text">文本</param>
                void Write(string text);

              
                /// <summary>
                /// 写入流
                /// </summary>
                /// <param name="stream"></param>
                void Write(Stream stream);
        }
}
