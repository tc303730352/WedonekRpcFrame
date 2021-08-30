
using HttpService.Interface;

namespace HttpApiGateway.Interface
{
        internal interface IApiController : IApiGateway
        {
                /// <summary>
                /// 服务名
                /// </summary>
                string ServiceName { get; }

                /// <summary>
                /// 用户状态
                /// </summary>
                dynamic UserState { get; }

                /// <summary>
                /// 请求
                /// </summary>
                IHttpRequest Request
                {
                        get;
                }
                /// <summary>
                /// 安装方法
                /// </summary>
                void Install(IRouteConfig config);
        }
}
