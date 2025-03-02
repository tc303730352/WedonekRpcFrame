using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Resource;
using RpcStore.RemoteModel.Resource.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ResourceEvent : IRpcApiService
    {
        private readonly IResourceService _Service;
        public ResourceEvent (IResourceService service)
        {
            this._Service = service;
        }

        public ResourceDto GetResource (GetResource obj)
        {
            return this._Service.Get(obj.Id);
        }
        public PagingResult<ResourceDatum> QueryResource (QueryResource query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }


    }
}
