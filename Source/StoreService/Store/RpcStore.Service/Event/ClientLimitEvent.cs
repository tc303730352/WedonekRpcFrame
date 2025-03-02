using RpcStore.RemoteModel.ClientLimit;
using RpcStore.RemoteModel.ClientLimit.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    /// <summary>
    /// 服务节点限流配置
    /// </summary>
    internal class ClientLimitEvent : IRpcApiService
    {
        private readonly IClientLimitService _Service;

        public ClientLimitEvent (IClientLimitService service)
        {
            this._Service = service;
        }

        public void DeleteClientLimit (DeleteClientLimit obj)
        {
            this._Service.Delete(obj.Id);
        }
        public ClientLimitModel[] GetBindClientLimit (GetBindClientLimit obj)
        {
            return this._Service.GetClientLimit(obj.ServerId);
        }
        public ClientLimitData GetClientLimit (GetClientLimit obj)
        {
            return this._Service.Get(obj.RpcMerId, obj.ServerId);
        }

        public void SyncClientLimit (SyncClientLimit obj)
        {
            this._Service.Sync(obj.Datum);
        }
    }
}
