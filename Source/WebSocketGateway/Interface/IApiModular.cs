using System;

using ApiGateway.Attr;
using ApiGateway.Interface;

using HttpWebSocket.Interface;
using HttpWebSocket.Model;

namespace WebSocketGateway.Interface
{
        /// <summary>
        /// Api模块
        /// </summary>
        public interface IApiModular :IModular
        {
                /// <summary>
                /// 模块配置
                /// </summary>
                IModularConfig Config { get; }

             
                /// <summary>
                /// 授权验证
                /// </summary>
                /// <param name="request"></param>
                /// <returns></returns>
                bool Authorize(RequestBody request);
                /// <summary>
                /// 授权完成
                /// </summary>
                /// <param name="service"></param>
                void AuthorizeComplate(IApiService service);
                /// <summary>
                /// 获取会话
                /// </summary>
                /// <param name="sessionId"></param>
                /// <returns></returns>
                ISession GetSession(Guid sessionId);
                /// <summary>
                /// 获取会话列表
                /// </summary>
                /// <param name="sessionId"></param>
                /// <returns></returns>
                ISession[] GetSession(Guid[] sessionId);
                /// <summary>
                /// 取消授权
                /// </summary>
                /// <param name="accreditId"></param>
                /// <param name="error"></param>
                void CancelAccredit(string accreditId, string error);

                /// <summary>
                /// 会话离线
                /// </summary>
                /// <param name="session"></param>
                void SessionOffline(IClientSession session);
                /// <summary>
                /// 查找授权的会话
                /// </summary>
                /// <param name="accreditId"></param>
                /// <returns></returns>
                ISession[] FindSession(string accreditId);
                /// <summary>
                /// 通过授权码和自定义名字查询
                /// </summary>
                /// <param name="accreditId"></param>
                /// <param name="name"></param>
                /// <returns></returns>
                ISession FindOnlineSession(string accreditId,string name);
                /// <summary>
                /// 查找会话
                /// </summary>
                /// <param name="find"></param>
                /// <returns></returns>
                ISession[] FindSession(Func<ISessionBody, bool> find);
                /// <summary>
                /// 批量发送
                /// </summary>
                /// <param name="find"></param>
                /// <returns></returns>
                IBatchSend BatchSend(Func<ISessionBody, bool> find);
                /// <summary>
                /// 批量发送
                /// </summary>
                /// <param name="sessionId"></param>
                /// <returns></returns>
                IBatchSend BatchSend(Guid[] sessionId);
        }
}
