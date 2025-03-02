using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    /// <summary>
    /// 全局限制插件
    /// </summary>
    internal class LimitPlugIn : BasicPlugin
    {
        private readonly IWholeLimitPlugIn _Limit;
        public LimitPlugIn ()
        {
            this._Limit = RpcClient.Ioc.Resolve<IWholeLimitPlugIn>();
            base._Init(this._Limit);
        }
        public override void Exec (IRoute route, IHttpHandler handler)
        {
            if (this._Limit.IsLimit())
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.TooManyRequests);
                handler.Response.End();
            }
        }
    }
}
