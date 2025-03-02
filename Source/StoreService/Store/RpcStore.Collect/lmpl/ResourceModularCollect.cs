using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.ResourceModular.Model;
namespace RpcStore.Collect.lmpl
{
    internal class ResourceModularCollect : IResourceModularCollect
    {
        private readonly IResourceModularDAL _BasicDAL;
        public ResourceModularCollect (IResourceModularDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public BasicModular[] Gets (long rpcMerId, string systemType)
        {
            return this._BasicDAL.Gets(rpcMerId, systemType);
        }
        public void SetRemark (ResourceModularModel modular, string remark)
        {
            if (modular.Remark == remark)
            {
                return;
            }
            this._BasicDAL.SetRemark(modular.Id, remark);
        }
        public ResourceModularModel[] Query (ModularQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }

        public ResourceModularModel Get (long modularId)
        {
            ResourceModularModel modular = this._BasicDAL.Get(modularId);
            if (modular == null)
            {
                throw new ErrorException("rpc.store.resource.modular.not.find");
            }
            return modular;
        }
    }
}
