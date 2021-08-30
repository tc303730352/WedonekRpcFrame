using RpcModel;

using RpcService.Logic;
using RpcService.Route;

namespace RpcService.Event
{
        internal class GetRemoteServerEvent : TcpRoute<GetRemoteServer, ServerConfigInfo>
        {
                public GetRemoteServerEvent() : base("GetRemoteServer")
                {

                }
                protected override bool ExecAction(GetRemoteServer param, out ServerConfigInfo result, out string error)
                {
                        return ServiceLogic.GetRemoteServer(param, out result, out error);
                }
        }
}

