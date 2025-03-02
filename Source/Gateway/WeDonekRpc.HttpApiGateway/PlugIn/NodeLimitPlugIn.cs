using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    /// <summary>
    /// 请求接口限制
    /// </summary>
    internal class NodeLimitPlugIn : BasicPlugin
    {
        private readonly INodeLimitPlugIn _Limit;


        public NodeLimitPlugIn ()
        {
            this._Limit = RpcClient.Ioc.Resolve<INodeLimitPlugIn>();
            base._Init(this._Limit);
        }
        public override void Exec (IRoute route, IHttpHandler handler)
        {
            if (this._Limit.IsLimit(route.ApiUri))
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.TooManyRequests);
                handler.Response.End();
            }
        }
    }
}
