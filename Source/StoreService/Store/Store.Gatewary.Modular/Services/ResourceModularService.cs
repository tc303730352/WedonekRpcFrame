using WeDonekRpc.Model;
using RpcStore.RemoteModel.ResourceModular;
using RpcStore.RemoteModel.ResourceModular.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ResourceModularService : IResourceModularService
    {
        public BasicModular[] GetBasicModular(long rpcMerId, string systemType)
        {
            return new GetBasicModular
            {
                RpcMerId = rpcMerId,
                SystemType = systemType,
            }.Send();
        }

        public ResourceModularDatum[] QueryModular(ModularQuery query, IBasicPage paging, out int count)
        {
            return new QueryModular
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetModularRemark(long id, string remark)
        {
            new SetModularRemark
            {
                Id = id,
                Remark = remark,
            }.Send();
        }

    }
}
