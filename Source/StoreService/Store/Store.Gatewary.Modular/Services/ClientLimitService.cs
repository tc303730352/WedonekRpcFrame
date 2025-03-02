using RpcStore.RemoteModel.ClientLimit;
using RpcStore.RemoteModel.ClientLimit.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ClientLimitService : IClientLimitService
    {
        public void DeleteClientLimit (long id)
        {
            new DeleteClientLimit
            {
                Id = id,
            }.Send();
        }
        public ClientLimitModel[] GetAllClientLimit (long serverId)
        {
            return new GetBindClientLimit
            {
                ServerId = serverId
            }.Send();
        }
        public ClientLimitData GetClientLimit (long rpcMerId, long serverId)
        {
            return new GetClientLimit
            {
                RpcMerId = rpcMerId,
                ServerId = serverId,
            }.Send();
        }

        public void SyncClientLimit (ClientLimitDatum datum)
        {
            new SyncClientLimit
            {
                Datum = datum,
            }.Send();
        }

    }
}
