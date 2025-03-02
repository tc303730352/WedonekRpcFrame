using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.CurConfig;
using RpcStore.RemoteModel.CurConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ServerCurConfigEvent : IRpcApiService
    {
        private readonly IServerCurConfigService _Service;

        public ServerCurConfigEvent (IServerCurConfigService service)
        {
            this._Service = service;
        }

        public CurConfigModel GetServerCurConfig (GetServerCurConfig obj)
        {
            return this._Service.Get(obj.ServerId);
        }
    }
}
