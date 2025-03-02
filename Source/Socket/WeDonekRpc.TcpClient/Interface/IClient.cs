using WeDonekRpc.TcpClient.Client;
using WeDonekRpc.TcpClient.Enum;
using WeDonekRpc.TcpClient.Model;

namespace WeDonekRpc.TcpClient.Interface
{
    internal interface IClient
    {
        SocketClient Client { get; }
        bool ConnectServer (string ipAddress, int port);
        int LastTime { get; }
        ClientStatus ClientStatus { get; }

        /// <summary>
        /// 服务器IP
        /// </summary>
        string ServerId { get; }


        /// <summary>
        /// 服务端授权完成
        /// </summary>
        void AuthorizationComplate ();

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        void CloseClientCon ();

        /// <summary>
        /// 检查客户端是否适合发送数据
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        bool CheckIsSend ();

        /// <summary>
        /// 在几秒后关闭客户端连接
        /// </summary>
        void CloseClientCon (int time);

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="page"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        bool SendPage (Page page, out string error);
        bool Send (DataPage page);
        void SendSystemPage (Page page);

    }
}
