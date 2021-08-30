
using RpcClient.Attr;
using RpcClient.Interface;

using RpcModularModel.Resource;
using RpcModularModel.Resource.Model;

using RpcHelper;

namespace RpcModular.Service
{
        [UnityName("ResourceService")]
        internal class RpcResourceService : IExtendService
        {
                public string Name => "ResourceService";
                private ITcpRouteCollect _Route = null;
                public void Load(IRpcService service)
                {
                        ResourceType range = RpcClient.RpcClient.Config.GetConfigVal<ResourceType>("resource:UpRange", (ResourceType)6);
                        if ((ResourceType.RPC接口 & range) == ResourceType.RPC接口)
                        {
                                service.StartUpComplate += this.Service_StartUpComplate;
                                this._Route = RpcClient.RpcClient.Unity.Resolve<ITcpRouteCollect>();
                        }
                }

                private void Service_StartUpComplate()
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
                                        ResourceService.Submit(RpcClient.RpcClient.SystemTypeVal, ResourceType.RPC接口, list);
                                }
                        }
                }

                private void _Route_RegRouteEvent(IRoute obj)
                {
                        if (!obj.IsSystemRoute)
                        {
                                ResourceService.Submit(RpcClient.RpcClient.SystemTypeVal, ResourceType.RPC接口, new ResourceDatum[] {
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
