using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.DAL
{
    public interface IResourceDAL
    {
        ResourceListModel Get(long id);
        ResourceListModel[] Query(ResourceQuery query, IBasicPage paging, out int count);
    }
}