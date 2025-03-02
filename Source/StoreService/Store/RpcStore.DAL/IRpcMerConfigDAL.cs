using RpcStore.Model.DB;
using RpcStore.Model.RpcMerConfig;
using RpcStore.RemoteModel.MerConfig.Model;

namespace RpcStore.DAL
{
    public interface IRpcMerConfigDAL
    {
        void Add (RpcMerConfigModel add);
        bool CheckIsExists (long rpcMerId, long sysTypeId);
        void Delete (long id);
        RpcMerConfigModel Get (long id);
        RpcMerConfig Get (long rpcMerId, long systemTypeId);
        RpcMerConfigModel[] Gets (long rpcMerId);
        void Set (long id, MerConfigSet config);
    }
}