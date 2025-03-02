using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.PlugIn
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
            base._Init(this._Limit, Interface.ExecStage.请求);
        }
        public override bool RequestInit (IApiService service, out string error)
        {
            if (this._Limit.IsLimit())
            {
                error = "http.request.exceed.limit";
                return false;
            }
            error = null;
            return true;
        }
    }
}
