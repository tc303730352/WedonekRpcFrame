using RpcStore.RemoteModel.CurConfig.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IServerCurConfigService
    {
        CurConfigModel Get (long serverId);
    }
}