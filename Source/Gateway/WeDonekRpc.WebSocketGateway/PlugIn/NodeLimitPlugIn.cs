using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Interface;

namespace WeDonekRpc.WebSocketGateway.PlugIn
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
            base._Init(this._Limit, Interface.ExecStage.执行);
        }
        public override bool Exec (IApiService service, ApiHandler handler, out string error)
        {
            if (this._Limit.IsLimit(handler.LocalPath))
            {
                error = "http.request.exceed.limit";
                return false;
            }
            error = null;
            return true;
        }

    }
}
