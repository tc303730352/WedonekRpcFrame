using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Visit.Model;

namespace WeDonekRpc.Modular.Service
{
    [IocName("RpcVisitService")]
    internal class RpcVisitService : IRpcExtendService
    {
        private IRouteService _Route;
        private IRpcService _Service;
        private volatile bool _IsEnable = false;
        public void Load (IRpcService service)
        {
            this._Service = service;
            service.StartUpComplate += this.Service_StartUpComplate;
            this._Route = RpcClient.Ioc.Resolve<IRouteService>();
            VisitService.StatusRefresh = this._StatusRefresh;
            if (VisitService.Config.IsEnable)
            {
                service.ReceiveEnd += this.Service_ReceiveEnd;
            }
        }

        private void _StatusRefresh (bool isEnable)
        {
            if (this._IsEnable == isEnable)
            {
                return;
            }
            this._IsEnable = isEnable;
            if (isEnable)
            {
                this._Service.ReceiveEnd += this.Service_ReceiveEnd;
            }
            else
            {
                this._Service.ReceiveEnd -= this.Service_ReceiveEnd;
            }
        }

        private void Service_StartUpComplate ()
        {
            this._Route.RegRouteEvent += this._Route_RegRouteEvent; ;
            IRoute[] routes = this._Route.GetRoutes();
            if (routes.Length > 0)
            {
                RpcVisit[] list = routes.ConvertAll(a => new RpcVisit
                {
                    Dictate = a.RouteName,
                    Show = a.RouteShow
                });
                if (list.Length > 0)
                {
                    VisitService.Regs(list);
                }
            }
        }

        private void _Route_RegRouteEvent (IRoute obj)
        {
            VisitService.Reg(new RpcVisit
            {
                Dictate = obj.RouteName,
                Show = obj.RouteShow
            });
        }

        private void Service_ReceiveEnd (IMsg msg, TcpRemoteReply reply)
        {
            if (msg.MsgKey == "AddVisitLog" && RpcClient.SystemTypeVal == "sys.sync")
            {
                return;
            }
            else if (reply != null && reply.IsError)
            {
                VisitService.Failure(msg.MsgKey);
            }
            else
            {
                VisitService.Success(msg.MsgKey);
            }
        }
    }
}
