
using System;
using System.Net;

using WeDonekRpc.TcpServer.Model;

namespace WeDonekRpc.TcpServer.Interface
{
    internal interface IClient
    {
        ISocketEvent Event { get; }
        /// <summary>
        /// 客户端ID
        /// </summary>
        Guid ClientId { get; }
        /// <summary>
        /// 服务端ID
        /// </summary>
        Guid ServerId { get; }
        /// <summary>
        /// 绑定参数
        /// </summary>
        string BindParam { get; }

        /// <summary>
        /// 客户端当前状态
        /// </summary>
        Enum.ClientStatus CurrentStatus { get; }

        /// <summary>
        /// 检查是否连接
        /// </summary>
        /// <returns></returns>
        void CheckIsCon (DateTime time);

        /// <summary>
        /// 检查客户端链接状态
        /// </summary>
        /// <returns></returns>
        bool CheckIsCon ();

        /// <summary>
        /// 服务端授权完成
        /// </summary>
        void AuthorizationComplate (string bindParam);

        /// <summary>
        /// 关闭客户端连接
        /// </summary>
        void CloseClientCon ();

        /// <summary>
        /// 在几秒后关闭客户端连接
        /// </summary>
        void CloseClientCon (int time);

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        IPEndPoint RempteIp { get; }

        bool Send (Page page, out string error);
        bool Send (ReplyPage page);
    }
}
