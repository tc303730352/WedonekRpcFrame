using RpcCentral.Service.Interface;
using RpcCentral.Service.Route;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class ContrainerLoginEvent : TcpRoute<RpcServerLogin, RpcServerLoginRes>
    {
        private readonly IContrainerLoginService _ServerLogin;
        public ContrainerLoginEvent (IContrainerLoginService server) : base()
        {
            this._ServerLogin = server;
        }
        protected override RpcServerLoginRes ExecAction (RpcServerLogin obj, string ip)
        {
            return this._ServerLogin.Login(obj, ip);
        }
    }
}
