using RpcStore.RemoteModel.ClientLimit.Model;

namespace RpcStore.Service.Interface
{
    public interface IClientLimitService
    {
        void Delete (long id);
        ClientLimitData Get (long rpcMerId, long serverId);
        ClientLimitModel[] GetClientLimit (long serverId);
        void Sync (ClientLimitDatum add);
    }
}