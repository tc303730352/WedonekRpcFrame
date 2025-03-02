using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.Service.Interface
{
    public interface IResourceService
    {
        ResourceDto Get (long id);
        PagingResult<ResourceDatum> Query (ResourceQuery query, IBasicPage paging);
    }
}