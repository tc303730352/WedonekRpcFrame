using System;

using HttpApiGateway.Interface;

using RpcModular;

using RpcModularModel.Resource;
using RpcModularModel.Resource.Model;

namespace HttpApiGateway.Collect
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
                                Uri uri = new Uri(ApiGatewayService.Config.Url, route.ApiUri);
                                string show = ApiGatewayService.GetApiShow(uri);
                                this._Container.Add(new ResourceDatum
                                {
                                        ResourcePath = route.ApiUri,
                                        FullPath = uri.AbsoluteUri,
                                        ResourceShow = show
                                });
                        }
                }

        }
}
