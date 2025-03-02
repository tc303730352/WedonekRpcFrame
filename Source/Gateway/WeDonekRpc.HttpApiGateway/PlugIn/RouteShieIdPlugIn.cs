using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService.Interface;

namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    internal class RouteShieIdPlugIn : BasicPlugin
    {
        private readonly IShieIdPlugIn _Service;
        public RouteShieIdPlugIn ()
        {
            this._Service = RpcClient.Ioc.Resolve<IShieIdPlugIn>();
            this._Init(this._Service);
        }

        public override void Exec (IRoute route, IHttpHandler handler)
        {
            if (this._Service.CheckIsShieId(route.ApiUri))
            {
                handler.Response.SetHttpStatus(System.Net.HttpStatusCode.NotFound);
                handler.Response.End();
            }
        }
    }
}
