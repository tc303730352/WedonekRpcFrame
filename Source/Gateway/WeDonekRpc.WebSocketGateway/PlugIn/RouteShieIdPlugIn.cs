using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.PlugIn
{
    internal class RouteShieIdPlugIn : BasicPlugin
    {
        private readonly IShieIdPlugIn _Service;
        public RouteShieIdPlugIn ()
        {
            this._Service = RpcClient.Ioc.Resolve<IShieIdPlugIn>();
            this._Init(this._Service, Interface.ExecStage.执行);
        }

        public override bool Exec (IApiService service, ApiHandler handler, out string error)
        {
            if (this._Service.CheckIsShieId(handler.LocalPath))
            {
                error = "http.request.path.shield";
                return false;
            }
            error = null;
            return true;
        }
    }
}
