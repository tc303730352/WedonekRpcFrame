using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.DAL.Repository
{
    internal class ResourceDAL : IResourceDAL
    {
        private IRpcExtendResource<ResourceListModel> _BasicDAL;
        public ResourceDAL(IRpcExtendResource<ResourceListModel> dal)
        {
            _BasicDAL = dal;
        }

        public ResourceListModel Get(long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public ResourceListModel[] Query(ResourceQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return _BasicDAL.Query(query.ToWhere(), paging, out count);
        }
    }
}
