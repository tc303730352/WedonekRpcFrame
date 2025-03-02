
using WeDonekRpc.Client;
using WeDonekRpc.Modular;

using WeDonekRpc.ModularModel.Resource;
using WeDonekRpc.ModularModel.Resource.Model;

using WeDonekRpc.WebSocketGateway.Interface;

namespace WeDonekRpc.WebSocketGateway.Collect
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

        public void RegRoute (ApiHandler route)
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
