using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.HttpApiGateway.Handler;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;
using WeDonekRpc.Modular;

namespace WeDonekRpc.HttpApiGateway
{
    /// <summary>
    /// 接口操作类
    /// </summary>
    public class ApiController : IApiController
    {
        private readonly IApiService _Service = HttpApiHandler.ApiService.Value;

        /// <summary>
        /// 服务名
        /// </summary>
        public string ServiceName => this._Service.ServiceName;
        /// <summary>
        /// 授权码
        /// </summary>
        public string AccreditId => this._Service.AccreditId;
        /// <summary>
        /// 用户状态
        /// </summary>
        public IUserState UserState => this._Service.UserState;
        /// <summary>
        /// 客户端标识
        /// </summary>
        public IUserIdentity Identity => this._Service.Identity;
        /// <summary>
        /// 请求
        /// </summary>
        public IHttpRequest Request => this._Service.Request;
        /// <summary>
        /// 基础服务对象
        /// </summary>
        public IApiService Service => this._Service;
        /// <summary>
        /// 请求状态
        /// </summary>
        public IState RequestState => this._Service.RequestState;
        /// <summary>
        /// 取消授权
        /// </summary>
        protected void CancelAccredit ()
        {
            if (this.AccreditId.IsNotNull())
            {
                IAccredit accredit = RpcClient.Ioc.Resolve<IAccredit>();
                accredit.CancelAccredit(this.AccreditId);
            }
        }

    }
}
