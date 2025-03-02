using System;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    /// <summary>
    /// 当前模块
    /// </summary>
    public interface ICurrentModular
    {
        /// <summary>
        /// 是否有值
        /// </summary>
        bool IsHasValue
        {
            get;
        }
        /// <summary>
        /// 服务名
        /// </summary>
        string ServerName { get; }
        /// <summary>
        /// 取消授权
        /// </summary>
        /// <param name="accreditId"></param>
        /// <param name="error"></param>
        void CancelAccredit (string accreditId, string error);
        /// <summary>
        /// 查找同一个授权会话
        /// </summary>
        /// <param name="accreditId"></param>
        /// <returns></returns>
        ISession[] FindSession (string accreditId);
        /// <summary>
        /// 查询会话
        /// </summary>
        /// <param name="find"></param>
        /// <returns></returns>
        ISession[] FindSession (Func<ISessionBody, bool> find);
        /// <summary>
        /// 查找在线的会话
        /// </summary>
        /// <param name="accreditId"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        ISession FindOnlineSession (string accreditId, string name);

        IBatchSend BatchSend (Func<ISessionBody, bool> find);
        IBatchSend BatchSend (Guid[] sessionId);

        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        ISession GetSession (Guid sessionId);
        ISession[] GetSession (Guid[] sessionId);
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="modular"></param>
        ICurrentModular Init (string modular);
    }
}