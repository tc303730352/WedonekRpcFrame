using RpcModel;

using RpcService.Logic;
using RpcService.Route;

namespace RpcService.Event
{
        internal class ServerLoginEvent : TcpRoute<RpcServerLogin, RpcServerLoginRes>
        {
                public ServerLoginEvent() : base("RemoteServerLogin")
                {

                }
                protected override bool ExecAction(RpcServerLogin obj, string ip, out RpcServerLoginRes result, out string error)
                {
                        return ServiceLogic.ServerLogin(obj, ip, out result, out error);
                }
        }
}
