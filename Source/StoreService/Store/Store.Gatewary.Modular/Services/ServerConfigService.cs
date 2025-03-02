using WeDonekRpc.Model;
using RpcStore.RemoteModel.ServerConfig;
using RpcStore.RemoteModel.ServerConfig.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class ServerConfigService : IServerConfigService
    {
        public long AddServer (ServerConfigAdd datum)
        {
            return new AddServer
            {
                Datum = datum,
            }.Send();
        }

        public void DeleteServer (long serverId)
        {
            new DeleteServer
            {
                ServerId = serverId,
            }.Send();
        }
        public RemoteServerModel GetServerDatum (long serverId)
        {
            return new GetServerDatum
            {
                Id = serverId
            }.Send();
        }
        public RemoteServerDatum GetServer (long serverId)
        {
            return new GetServer
            {
                ServerId = serverId,
            }.Send();
        }

        public RemoteServer[] QueryServer (ServerConfigQuery query, IBasicPage paging, out int count)
        {
            return new QueryServer
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void SetServer (long serverId, ServerConfigSet datum)
        {
            new SetServer
            {
                ServerId = serverId,
                Datum = datum,
            }.Send();
        }

        public void SetServiceState (long serviceId, RpcServiceState state)
        {
            new SetServiceState
            {
                ServiceId = serviceId,
                State = state,
            }.Send();
        }

        public ServerItem[] GetItems (ServerConfigQuery query)
        {
            return new GetServerItems
            {
                Query = query
            }.Send();
        }

        public void SetVerNum (long id, int verNum)
        {
            new SetServerVerNum
            {
                Id = id,
                VerNum = verNum
            }.Send();
        }
    }
}
