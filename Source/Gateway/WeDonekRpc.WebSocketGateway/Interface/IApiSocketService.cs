using WeDonekRpc.HttpWebSocket.Model;

using WeDonekRpc.Modular;

namespace WeDonekRpc.WebSocketGateway.Interface
{
    public interface IApiSocketService
    {
        /// <summary>
        /// 授权码
        /// </summary>
        string AccreditId { get; }

        /// <summary>
        /// 请求头
        /// </summary>
        RequestBody Head { get; }
        /// <summary>
        /// 身份标识
        /// </summary>
        IUserIdentity Identity { get; }


        /// <summary>
        /// 身份Id
        /// </summary>
        string IdentityId { get; }
        /// <summary>
        /// 服务名
        /// </summary>
        string ServiceName { get; }
        /// <summary>
        /// 会话标识
        /// </summary>
        ISession Session { get; }
        /// <summary>
        /// 用户状态
        /// </summary>
        IUserState UserState { get; }
    }
}