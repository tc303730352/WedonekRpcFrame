using HttpWebSocket.Model;

using WebSocketGateway;
using WebSocketGateway.Interface;
using WebSocketGateway.Model;

namespace Wedonek.Gateway.WebSocket
{
        /// <summary>
        ///WebSocketGateway网关模块
        /// </summary>
        public class WebSocketGatewayModular : BasicApiModular
        {
                public WebSocketGatewayModular() : base("WebSocketDemo")
                {

                }
                /// <summary>
                /// 模块初始化
                /// </summary>
                protected override void Init()
                {
                        this.Config.RegUserState<UserLoginState>();
                }
                /// <summary>
                /// 验证WebSocket请求是否有效
                /// </summary>
                /// <param name="request"></param>
                /// <returns></returns>
                public override bool Authorize(RequestBody request)
                {
                        return base.Authorize(request);
                }
                /// <summary>
                /// 用户验证
                /// </summary>
                /// <param name="request"></param>
                /// <param name="session"></param>
                protected override AuthResult? UserAuthorize(RequestBody request, ISession session)
                {
                        return base.UserAuthorize(request, session);
                }

        }
}
