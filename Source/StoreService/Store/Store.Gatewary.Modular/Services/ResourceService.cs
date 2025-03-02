using WeDonekRpc.Model;
using RpcStore.RemoteModel.Resource;
using RpcStore.RemoteModel.Resource.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ResourceService : IResourceService
    {
        public ResourceDto Get (long id)
        {
            return new GetResource
            {
                Id = id
            }.Send();
        }
        public ResourceDatum[] QueryResource (ResourceQuery query, IBasicPage paging, out int count)
        {
            return new QueryResource
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

    }
}
