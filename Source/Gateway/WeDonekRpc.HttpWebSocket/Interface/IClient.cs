using System.IO;

namespace WeDonekRpc.HttpWebSocket.Interface
{
        public interface IClient
        {
                /// <summary>
                /// 是否连接
                /// </summary>
                bool IsCon { get; }
                /// <summary>
                /// 发送文本
                /// </summary>
                /// <param name="text"></param>
                bool Send(string text);
                /// <summary>
                /// 发送数据流
                /// </summary>
                /// <param name="stream"></param>
                bool Send(Stream stream);
        }
}