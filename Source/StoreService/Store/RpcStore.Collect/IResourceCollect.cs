using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.Collect
{
    public interface IResourceCollect
    {
        ResourceListModel Get(long id);
        ResourceListModel[] Query(ResourceQuery query, IBasicPage paging, out int count);
    }
}