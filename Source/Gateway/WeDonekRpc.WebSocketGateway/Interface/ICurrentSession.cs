using System;

namespace WeDonekRpc.WebSocketGateway.Interface
{
        /// <summary>
        /// 当前会话
        /// </summary>
        public interface ICurrentSession : ISession
        {
                /// <summary>
                /// 是否有值
                /// </summary>
                bool IsHasValue { get; }
                /// <summary>
                /// 初始化会话
                /// </summary>
                /// <param name="modular">模块名</param>
                /// <param name="sessionId">会话Id</param>
                /// <returns>会话</returns>
                ICurrentSession InitSession(string modular, Guid sessionId);
        }
}