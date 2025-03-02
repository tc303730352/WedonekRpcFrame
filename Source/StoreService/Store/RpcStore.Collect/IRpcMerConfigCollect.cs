using RpcStore.Model.DB;
using RpcStore.Model.RpcMerConfig;
using RpcStore.RemoteModel.MerConfig.Model;

namespace RpcStore.Collect
{
    public interface IRpcMerConfigCollect
    {
        long Add (MerConfigArg add);
        void Delete (RpcMerConfigModel config);
        RpcMerConfigModel GetConfig (long id);
        RpcMerConfig GetConfig (long rpcMerId, long systemTypeId);
        RpcMerConfigModel[] GetConfigs (long rpcMerId);
        bool Set (RpcMerConfig config, MerConfigSet param);
    }
}