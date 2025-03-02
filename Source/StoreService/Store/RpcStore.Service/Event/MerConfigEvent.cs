using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.MerConfig;
using RpcStore.RemoteModel.MerConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class MerConfigEvent : IRpcApiService
    {
        private readonly IRpcMerConfigService _Service;
        public MerConfigEvent (IRpcMerConfigService service)
        {
            this._Service = service;
        }

        public long SetMerConfig (SetMerConfig add)
        {
            return this._Service.Set(add.Config);
        }

        public void DeleteMerConfig (DeleteMerConfig obj)
        {
            this._Service.Delete(obj.Id);
        }

        public RpcMerConfig GetMerConfig (GetMerConfig obj)
        {
            return this._Service.GetConfig(obj.RpcMerId, obj.SystemTypeId);
        }

        public RpcMerConfigDatum[] GetMerConfigByMerId (GetMerConfigByMerId obj)
        {
            return this._Service.GetConfigs(obj.RpcMerId);
        }

    }
}
