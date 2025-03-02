using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.PlugIn
{

    internal class IpLimitPlugIn : BasicPlugin
    {
        private readonly IIpLimitPlugIn _Limit;
        public IpLimitPlugIn ()
        {
            this._Limit = RpcClient.Ioc.Resolve<IIpLimitPlugIn>();
            this._Init(this._Limit, Interface.ExecStage.请求);
        }

        public override bool RequestInit (IApiService service, out string error)
        {
            if (this._Limit.IsLimit(service.Request.Head.RemoteIp))
            {
                error = "http.request.ip.exceed.limit";
                return false;
            }
            error = null;
            return true;
        }
    }
}
