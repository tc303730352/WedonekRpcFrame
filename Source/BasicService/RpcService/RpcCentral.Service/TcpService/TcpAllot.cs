using RpcCentral.Common;
using RpcCentral.Service.Interface;
using RpcCentral.Service.Model;
using WeDonekRpc.Model;
using WeDonekRpc.TcpServer.Interface;

namespace RpcCentral.Service.TcpService
{
    internal class TcpAllot : IAllot
    {
        public override object Action ()
        {
            using (IocScope scope = UnityHelper.CreateScore())
            {
                ITcpRoute route = TcpRouteService.GetRoute(this.Type);
                if (route == null)
                {
                    return new BasicRes("rpc.direct.no.reg");
                }
                return route.TcpMsgEvent(new RemoteMsg(base.GetData(), base.ClientIp.Address.ToString()));
            }
        }
    }
}
