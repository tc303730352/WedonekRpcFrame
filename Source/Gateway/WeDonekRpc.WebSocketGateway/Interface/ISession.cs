using System.IO;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface ISession : ISessionBody
    {
        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="error"></param>
        void CancelAccredit (string error = null);
        /// <summary>
        /// 发送系统错误信息
        /// </summary>
        /// <param name="error"></param>
        void Send (ErrorException error);
        /// <summary>
        /// 发送错误信息
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="error"></param>
        void Send (string direct, ErrorException error);
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="direct"></param>
        /// <param name="data"></param>
        void Send (string direct, object data);
        /// <summary>
        /// 发送流
        /// </summary>
        /// <param name="stream"></param>
        void Send (Stream stream);
        /// <summary>
        /// 设置会话名称
        /// </summary>
        /// <param name="name"></param>
        void SetName (string name);
    }
}