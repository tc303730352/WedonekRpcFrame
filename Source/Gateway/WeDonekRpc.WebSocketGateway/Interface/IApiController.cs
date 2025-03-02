using WeDonekRpc.HttpWebSocket.Model;
using WeDonekRpc.Modular;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IApiController : IApiGateway
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
        /// 用户标识
        /// </summary>
        IUserIdentity Identity { get; }

        /// <summary>
        /// 请求头
        /// </summary>
        RequestBody Head
        {
            get;
        }

        /// <summary>
        /// 安装方法
        /// </summary>
        void Install (IRouteConfig config);
    }
}
