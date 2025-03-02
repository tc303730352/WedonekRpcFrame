using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper.Error;
using WeDonekRpc.Model;
using WeDonekRpc.Model.ErrorManage;

namespace WeDonekRpc.Client.Error
{
    [Attr.IocName("ErrorService")]
    internal class ErrorExtendService : IRpcExtendService
    {
        private readonly IErrorService _Service;
        private readonly IRouteService _RouteService;
        public ErrorExtendService (IErrorService service, IRouteService routeService)
        {
            this._Service = service;
            this._RouteService = routeService;
        }
        //protected override 
        public void Load (IRpcService service)
        {
            service.InitComplating += this.Service_InitComplating;
        }
        private void Service_InitComplating ()
        {
            if (Config.WebConfig.BasicConfig.RpcSystemType == "sys.sync")
            {
                return;
            }
            else if (Config.WebConfig.BasicConfig.IsEnableError)
            {
                LocalErrorManage.SetAction(new ErrorEvent(this._Service));
                this._RouteService.AddRoute<RefreshError>("RefreshError", _Refresh);
            }
        }

        private static void _Refresh (RefreshError data, MsgSource source)
        {
            IErrorService error = RpcClient.Ioc.Resolve<IErrorService>();
            error.Refresh(data.ErrorId);
        }
    }
}
