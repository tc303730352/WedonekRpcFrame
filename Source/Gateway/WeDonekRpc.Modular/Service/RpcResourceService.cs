
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.ModularModel.Resource;
using WeDonekRpc.ModularModel.Resource.Model;

namespace WeDonekRpc.Modular.Service
{
    [IocName("ResourceService")]
    internal class RpcResourceService : IRpcExtendService
    {
        private IRouteService _Route = null;
        public void Load (IRpcService service)
        {
            ResourceType range = RpcClient.Config.GetConfigVal<ResourceType>("rpcassembly:resource:UpRange", (ResourceType)6);
            if (( ResourceType.RPC接口 & range ) == ResourceType.RPC接口)
            {
                service.StartUpComplate += this.Service_StartUpComplate;
                this._Route = RpcClient.Ioc.Resolve<IRouteService>();
            }
        }

        private void Service_StartUpComplate ()
        {
            this._Route.RegRouteEvent += this._Route_RegRouteEvent;
            IRoute[] routes = this._Route.GetRoutes();
            if (routes.Length > 0)
            {
                ResourceDatum[] list = routes.Convert(a => a.IsSystemRoute == false, a => new ResourceDatum
                {
                    FullPath = a.ToString(),
                    ResourceShow = a.RouteShow,
                    ResourcePath = a.RouteName
                });
                if (list.Length > 0)
                {
                    ResourceService.Submit(RpcClient.SystemTypeVal, ResourceType.RPC接口, list);
                }
            }
        }

        private void _Route_RegRouteEvent (IRoute obj)
        {
            if (!obj.IsSystemRoute)
            {
                ResourceService.Submit(RpcClient.SystemTypeVal, ResourceType.RPC接口, new ResourceDatum[] {
                                        new ResourceDatum
                                        {
                                                FullPath = obj.ToString(),
                                                ResourceShow = obj.RouteShow,
                                                ResourcePath = obj.RouteName
                                        }
                                });
            }
        }

    }
}
