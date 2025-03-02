using RpcStore.RemoteModel.MerConfig;
using RpcStore.RemoteModel.MerConfig.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class MerConfigService : IMerConfigService
    {
        public long SetMerConfig (MerConfigArg config)
        {
            return new SetMerConfig
            {
                Config = config,
            }.Send();
        }

        public void DeleteMerConfig (long id)
        {
            new DeleteMerConfig
            {
                Id = id,
            }.Send();
        }

        public RpcMerConfig GetMerConfig (long rpcMerId, long systemTypeId)
        {
            return new GetMerConfig
            {
                RpcMerId = rpcMerId,
                SystemTypeId = systemTypeId
            }.Send();
        }

        public RpcMerConfigDatum[] GetMerConfigByMerId (long rpcMerId)
        {
            return new GetMerConfigByMerId
            {
                RpcMerId = rpcMerId,
            }.Send();
        }

    }
}
