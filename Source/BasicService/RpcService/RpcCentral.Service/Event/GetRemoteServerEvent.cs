using RpcCentral.Service.Interface;
using RpcCentral.Service.Route;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class GetRemoteServerEvent : TcpRoute<GetRemoteServer, ServerConfigInfo>
    {
        private readonly IGetServerNodeService _Service;
        public GetRemoteServerEvent (IGetServerNodeService service) : base()
        {
            this._Service = service;
        }
        protected override ServerConfigInfo ExecAction (GetRemoteServer param)
        {
            return this._Service.Get(param.ServerId, param.CurServerId);
        }
    }
}

