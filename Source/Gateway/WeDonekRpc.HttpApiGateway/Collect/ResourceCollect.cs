using System;
using WeDonekRpc.Client;
using WeDonekRpc.HttpApiGateway.Interface;

using WeDonekRpc.Modular;

using WeDonekRpc.ModularModel.Resource;
using WeDonekRpc.ModularModel.Resource.Model;

namespace WeDonekRpc.HttpApiGateway.Collect
{
    internal class ResourceCollect : IResourceCollect
    {
        private static readonly IResourceService _Service = RpcClient.Ioc.Resolve<IResourceService>();


        public static IResourceCollect Create (IApiModular modular)
        {
            return new ResourceCollect(modular.ServiceName);
        }
        private readonly IResourceContainer _Container = null;
        public ResourceCollect (string name)
        {
            this._Container = _Service.GetContainer(name, ResourceType.API接口);
        }


        public void Dispose ()
        {
            this._Container?.Dispose();
        }

        public void RegRoute (IRoute route)
        {
            if (this._Container != null)
            {
                Uri uri = new Uri(ApiGatewayService.Config.RealRequestUri, route.ApiUri);
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
