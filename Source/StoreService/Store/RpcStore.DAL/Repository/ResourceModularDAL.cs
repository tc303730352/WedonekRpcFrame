using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.ResourceModular.Model;

namespace RpcStore.DAL.Repository
{
    internal class ResourceModularDAL : IResourceModularDAL
    {
        private readonly IRpcExtendResource<ResourceModularModel> _BasicDAL;
        public ResourceModularDAL (IRpcExtendResource<ResourceModularModel> dal)
        {
            this._BasicDAL = dal;
        }
        public void SetRemark (long id, string remark)
        {
            if (!this._BasicDAL.Update(c => c.Remark == remark, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.resource.modular.set.fail");
            }
        }

        public ResourceModularModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }

        public BasicModular[] Gets (long rpcMerId, string systemType)
        {
            return this._BasicDAL.Gets<BasicModular>(c => c.RpcMerId == rpcMerId && c.SystemType == systemType);
        }
        public ResourceModularModel[] Query (ModularQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }
    }
}
