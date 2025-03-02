using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model.Model;
using RpcStore.RemoteModel.LimitConfig;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class LimitConfigEvent : IRpcApiService
    {
        private IServerLimitConfigService _Service;

        public LimitConfigEvent(IServerLimitConfigService service)
        {
            _Service = service;
        }

        public void DeleteLimitConfig(DeleteLimitConfig obj)
        {
            _Service.Delete(obj.ServerId);
        }

        public ServerLimitConfig GetLimitConfig(GetLimitConfig obj)
        {
            return _Service.GetLimitConfig(obj.ServerId);
        }

        public void SyncLimitConfig(SyncLimitConfig obj)
        {
            _Service.SyncConfig(obj.Config);
        }
    }
}
