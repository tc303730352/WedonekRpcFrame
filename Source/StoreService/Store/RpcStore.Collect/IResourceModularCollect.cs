using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.ResourceModular.Model;

namespace RpcStore.Collect
{
    public interface IResourceModularCollect
    {
        ResourceModularModel Get(long modularId);
        BasicModular[] Gets(long rpcMerId, string systemType);
        ResourceModularModel[] Query(ModularQuery query, IBasicPage paging, out int count);
        void SetRemark(ResourceModularModel modular, string remark);
    }
}