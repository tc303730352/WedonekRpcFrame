using RpcStore.RemoteModel.CurConfig;
using RpcStore.RemoteModel.CurConfig.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ServerCurConfigService : IServerCurConfigService
    {
        public CurConfigModel Get (long serverId)
        {
            return new GetServerCurConfig
            {
                ServerId = serverId
            }.Send();
        }
    }
}
