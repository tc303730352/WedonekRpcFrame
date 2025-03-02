using RpcCentral.Service.Interface;
using RpcCentral.Service.Route;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class ServerLoginEvent : TcpRoute<RpcServerLogin, RpcServerLoginRes>
    {
        private IServerLoginService _ServerLogin;
        public ServerLoginEvent(IServerLoginService server) : base()
        {
            _ServerLogin = server;
        }
        protected override RpcServerLoginRes ExecAction(RpcServerLogin obj, string ip)
        {
            return _ServerLogin.Login(obj, ip);
        }
    }
}
