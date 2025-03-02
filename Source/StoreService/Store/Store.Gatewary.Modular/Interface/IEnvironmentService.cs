using RpcStore.RemoteModel.Environment.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IEnvironmentService
    {
        ServerEnvironment Get (long serverId);
    }
}