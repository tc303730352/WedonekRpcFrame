using System;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    public interface IService
    {
        /// <summary>
        /// 服务Id
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Socket配置
        /// </summary>
        IWebSocketConfig Config { get; }


        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        IClientSession GetSession (Guid sessionId);
        /// <summary>
        /// 获取会话信息
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        IClientSession[] GetSession (Guid[] sessionId);
        /// <summary>
        /// 查找会话
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        IClientSession[] FindSession (Func<ISessionBody, bool> find);
        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="accreditId"></param>
        /// <param name="error"></param>
        void CancelAccredit (string accreditId, string error);
        /// <summary>
        /// 获取同用户的所有会话
        /// </summary>
        /// <param name="accreditId"></param>
        /// <returns></returns>
        IClientSession[] FindSession (string accreditId);
        /// <summary>
        /// 关闭服务
        /// </summary>
        void Close ();
        IClientSession FindOnlineSession (string accreditId, string name);
    }
}
