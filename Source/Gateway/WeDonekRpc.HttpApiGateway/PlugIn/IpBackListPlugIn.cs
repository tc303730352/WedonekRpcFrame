using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    /// <summary>
    /// IP黑名单
    /// </summary>
    internal class IpBackPlugIn : BasicPlugin
    {
        private readonly IIpBlackListPlugIn _Limit;
        public IpBackPlugIn ()
        {
            this._Limit = RpcClient.Ioc.Resolve<IIpBlackListPlugIn>();
            this._Init(this._Limit);
        }

        public override void Exec (IRoute route, IHttpHandler handler)
        {
            if (this._Limit.IsLimit(handler.Request.ClientIp))
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.Forbidden);
                handler.Response.End();
            }
        }
    }
}
