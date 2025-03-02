using System;

namespace WeDonekRpc.HttpWebSocket.Interface
{
    public interface ISessionBody
    {
        /// <summary>
        /// 自定义名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 是否在线
        /// </summary>
        bool IsOnline { get; }
        /// <summary>
        /// 离线时间
        /// </summary>
        int OfflineTime { get; }
        /// <summary>
        /// 会话Id
        /// </summary>
        Guid SessionId { get; }
        /// <summary>
        /// 授权码
        /// </summary>
        string AccreditId { get; }
        /// <summary>
        /// 标识Id
        /// </summary>
        string IdentityId { get; }
        /// <summary>
        /// 是否已授权
        /// </summary>
        bool IsAccredit { get; }
    }
}