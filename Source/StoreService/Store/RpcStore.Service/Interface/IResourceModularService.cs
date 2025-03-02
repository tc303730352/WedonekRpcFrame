using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.ResourceModular.Model;

namespace RpcStore.Service.Interface
{
    public interface IResourceModularService
    {
        ResourceModularModel Get (long modularId);
        BasicModular[] Gets(long rpcMerId, string systemType);
        PagingResult<ResourceModularDatum> Query(ModularQuery query, IBasicPage paging);
        void SetRemark(long id, string remark);
    }
}