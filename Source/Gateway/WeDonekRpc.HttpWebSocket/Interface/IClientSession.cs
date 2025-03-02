using System.IO;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    public interface IClientSession : ISessionBody
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="accreditId"></param>
        /// <param name="identityId"></param>
        void Accredit ( string accreditId, string identityId );
        /// <summary>
        /// 设置会话名称
        /// </summary>
        /// <param name="name"></param>
        void SetName ( string name );
        /// <summary>
        /// /发送文本
        /// </summary>
        /// <param name="text"></param>
        bool Send ( string text );
        /// <summary>
        /// 发送数据流
        /// </summary>
        /// <param name="stream"></param>
        bool Send ( Stream stream );

        /// <summary>
        /// 取消授权
        /// </summary>
        void CancelAccredit ( string error );

    }
}