using RpcStore.RemoteModel.MerConfig.Model;

namespace RpcStore.Service.Interface
{
    public interface IRpcMerConfigService
    {
        void Delete (long id);
        RpcMerConfig GetConfig (long rpcMerId, long systemTypeId);
        RpcMerConfigDatum[] GetConfigs (long rpcMerId);
        long Set (MerConfigArg param);
    }
}