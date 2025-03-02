using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class ResourceShieldDAL : IResourceShieldDAL
    {
        private readonly IRpcExtendResource<ResourceShieldModel> _BasicDAL;
        public ResourceShieldDAL (IRpcExtendResource<ResourceShieldModel> dal)
        {
            this._BasicDAL = dal;
        }


        public void Delete (long[] ids)
        {
            _ = this._BasicDAL.Delete(a => ids.Contains(a.ResourceId));
        }
    }
}
