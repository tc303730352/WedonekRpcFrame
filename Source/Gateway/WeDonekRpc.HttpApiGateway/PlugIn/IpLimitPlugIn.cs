using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    /// <summary>
    /// Ip限制插件
    /// </summary>
    internal class IpLimitPlugIn : BasicPlugin
    {
        private readonly IIpLimitPlugIn _Limit;
        public IpLimitPlugIn ()
        {
            this._Limit = RpcClient.Ioc.Resolve<IIpLimitPlugIn>();
            this._Init(this._Limit);
        }

        public override void Exec (IRoute route, IHttpHandler handler)
        {
            if (this._Limit.IsLimit(handler.Request.ClientIp))
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.TooManyRequests);
                handler.Response.End();
            }
        }
    }
}
