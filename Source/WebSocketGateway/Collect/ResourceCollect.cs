
using RpcModular;

using RpcModularModel.Resource;
using RpcModularModel.Resource.Model;

using WebSocketGateway.Interface;

namespace WebSocketGateway.Collect
{
        internal class ResourceCollect : IResourceCollect
        {
                private static readonly IResourceService _Service = RpcClient.RpcClient.Unity.Resolve<IResourceService>();


                public static IResourceCollect Create(IApiModular modular)
                {
                        return new ResourceCollect(modular.ServiceName);
                }
                private readonly IResourceContainer _Container = null;
                public ResourceCollect(string name)
                {
                        this._Container = _Service.GetContainer(name, ResourceType.API接口);
                }


                public void Dispose()
                {
                        this._Container?.Dispose();
                }

                public void RegRoute(IRoute route)
                {
                        if (this._Container != null)
                        {
                                string show = GatewayService.GetApiShow(route.LocalPath);
                                this._Container.Add(new ResourceDatum
                                {
                                        ResourcePath = route.LocalPath,
                                        FullPath = route.LocalPath,
                                        ResourceShow = show
                                });
                        }
                }

        }
}
