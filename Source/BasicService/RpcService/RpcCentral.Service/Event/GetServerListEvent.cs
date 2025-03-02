using RpcCentral.Service.Interface;
using WeDonekRpc.Model.Server;

namespace RpcCentral.Service.Event
{
    internal class GetServerListEvent : Route.TcpRoute<GetServerList, GetServerListRes>
    {
        private readonly IGetServiceListService _Service;
        public GetServerListEvent (IGetServiceListService service) : base()
        {
            this._Service = service;
        }
        protected override GetServerListRes ExecAction (GetServerList param)
        {
            return this._Service.GetServerList(param, param.Source);
        }
    }
}
