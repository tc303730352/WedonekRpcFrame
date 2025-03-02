using System;

using WeDonekRpc.HttpService.Interface;

using WeDonekRpc.Modular;
namespace WeDonekRpc.HttpApiGateway.Interface
{

    public interface IApiService : IHttpHandler, IRequestState
    {
        /// <summary>
        /// 是否已截止响应
        /// </summary>
        bool IsEnd { get; }
        /// <summary>
        /// 请求来源
        /// </summary>
        Uri UrlReferrer { get; }
        /// <summary>
        /// 当前所在模块名
        /// </summary>
        string ServiceName { get; }
        /// <summary>
        /// 授权码
        /// </summary>
        string AccreditId { get; }

        /// <summary>
        /// 用户状态
        /// </summary>
        IUserState UserState { get; }
        /// <summary>
        /// 身份标识
        /// </summary>
        IUserIdentity Identity { get; }

    }
}
