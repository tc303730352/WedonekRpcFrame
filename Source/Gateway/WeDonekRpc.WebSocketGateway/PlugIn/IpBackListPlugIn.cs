using WeDonekRpc.ApiGateway.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.HttpWebSocket.Model;

namespace WeDonekRpc.WebSocketGateway.PlugIn
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
            this._Init(this._Limit, Interface.ExecStage.认证);
        }

        public override bool Authorize (RequestBody request)
        {
            return !this._Limit.IsLimit(request.RemoteIp);
        }
    }
}
