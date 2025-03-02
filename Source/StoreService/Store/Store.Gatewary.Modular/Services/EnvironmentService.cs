using RpcStore.RemoteModel.Environment;
using RpcStore.RemoteModel.Environment.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class EnvironmentService : IEnvironmentService
    {
        public ServerEnvironment Get (long serverId)
        {
            return new GetEnvironment
            {
                ServerId = serverId
            }.Send();
        }
    }
}
