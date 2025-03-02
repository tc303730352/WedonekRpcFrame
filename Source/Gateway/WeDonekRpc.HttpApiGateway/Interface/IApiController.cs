
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Modular;

namespace WeDonekRpc.HttpApiGateway.Interface
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
        IUserState UserState { get; }

        /// <summary>
        /// 请求
        /// </summary>
        IHttpRequest Request
        {
            get;
        }
    }
}
