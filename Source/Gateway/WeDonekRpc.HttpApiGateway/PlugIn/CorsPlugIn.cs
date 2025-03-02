using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;
using WeDonekRpc.HttpService;
using WeDonekRpc.HttpService.Interface;
namespace WeDonekRpc.HttpApiGateway.PlugIn
{
    internal class CorsPlugIn : IHttpPlugIn
    {
        private readonly ICrossConfig _Config;

        public bool IsEnable => this._Config.IsEnable;

        public string Name => "CorsPlugIn";

        public CorsPlugIn ()
        {
            this._Config = RpcClient.Ioc.Resolve<ICrossConfig>();
        }

        public void Init ()
        {

        }
        public void Exec (IRoute route, IHttpHandler handler)
        {
            this._Config.CheckCross(handler);
        }

        public void Dispose ()
        {

        }
    }
}
