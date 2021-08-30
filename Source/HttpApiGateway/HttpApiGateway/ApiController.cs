using ApiGateway.Interface;

using HttpApiGateway.Handler;
using HttpApiGateway.Interface;

using HttpService.Interface;

using RpcModular;

namespace HttpApiGateway
{
        /// <summary>
        /// 接口操作类
        /// </summary>
        public class ApiController : IApiController
        {
                private readonly IApiService _Service = HttpApiHandler.ApiService;

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
                public dynamic UserState => this._Service.UserState;
                /// <summary>
                /// 客户端标识
                /// </summary>
                public IClientIdentity Identity => this._Service.Identity;
                /// <summary>
                /// 请求
                /// </summary>
                public IHttpRequest Request => this._Service.Request;
                /// <summary>
                /// 基础服务对象
                /// </summary>
                public IApiService Service => this._Service;

                /// <summary>
                /// 初始化
                /// </summary>
                public virtual void Install(IRouteConfig config)
                {

                }
                /// <summary>
                /// 取消授权
                /// </summary>
                protected void CancelAccredit()
                {
                        IAccreditService accredit = RpcClient.RpcClient.Unity.Resolve<IAccreditService>();
                        accredit.CancelAccredit(this.AccreditId);
                }

        }
}
