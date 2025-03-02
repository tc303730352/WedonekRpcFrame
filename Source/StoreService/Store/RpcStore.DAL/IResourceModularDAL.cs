using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.ResourceModular.Model;

namespace RpcStore.DAL
{
    public interface IResourceModularDAL
    {
        ResourceModularModel Get(long id);
        BasicModular[] Gets(long rpcMerId, string systemType);
        ResourceModularModel[] Query(ModularQuery query, IBasicPage paging, out int count);
        void SetRemark(long id, string remark);
    }
}