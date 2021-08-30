
using System.Text;

using HttpWebSocket.Interface;

using RpcModular;
using RpcModular.Model;

namespace WebSocketGateway.Interface
{
        /// <summary>
        /// 模块配置
        /// </summary>
        public interface IModularConfig
        {
                /// <summary>
                /// 是否认证
                /// </summary>
                bool IsAccredit { get; }
                /// <summary>
                ///   Api 接口地址生成格式
                /// </summary>
                string ApiRouteFormat { get; set; }
                /// <summary>
                /// Socket配置
                /// </summary>
                IWebSocketConfig SocketConfig { get; }

                /// <summary>
                /// 响应模板
                /// </summary>
                IResponseTemplate ResponseTemplate { get; set; }
                /// <summary>
                /// 请求编码
                /// </summary>
                Encoding RequestEncoding { get; }

                /// <summary>
                /// 注册用户状态对象
                /// </summary>
                /// <typeparam name="T"></typeparam>
                void RegUserState<T>() where T : UserState;
                /// <summary>
                /// 获取用户授权状态
                /// </summary>
                /// <param name="accreditId">状态吗</param>
                /// <returns>授权状态</returns>
                IUserState GetAccredit(string accreditId);
                /// <summary>
                /// 检查授权状态
                /// </summary>
                /// <param name="authorizationId"></param>
                void CheckAccredit(string authorizationId);
        }
}