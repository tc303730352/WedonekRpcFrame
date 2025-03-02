using RpcStore.RemoteModel.CurConfig.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerCurConfigService
    {
        CurConfigModel Get (long serverId);
    }
}