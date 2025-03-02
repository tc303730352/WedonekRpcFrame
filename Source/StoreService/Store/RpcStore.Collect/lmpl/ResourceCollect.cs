using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Resource.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ResourceCollect : IResourceCollect
    {
        private IResourceDAL _BasicDAL;
        public ResourceCollect(IResourceDAL basicDAL)
        {
            _BasicDAL = basicDAL;
        }

        public ResourceListModel Get(long id)
        {
            ResourceListModel resource = this._BasicDAL.Get(id);
            if (resource == null)
            {
                throw new ErrorException("rpc.store.resource.not.find");
            }
            return resource;
        }

        public ResourceListModel[] Query(ResourceQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
    }
}
