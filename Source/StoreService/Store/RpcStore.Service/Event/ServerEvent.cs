using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ServerConfig;
using RpcStore.RemoteModel.ServerConfig.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class ServerEvent : IRpcApiService
    {
        private readonly IServerService _Service;
        public ServerEvent (IServerService service)
        {
            this._Service = service;
        }
        public void SetServerVerNum (SetServerVerNum obj)
        {
            this._Service.SetVerNum(obj.Id, obj.VerNum);
        }
        public long AddServer (AddServer add)
        {
            return this._Service.Add(add.Datum);
        }
        public string GetServerName (GetServerName obj)
        {
            return this._Service.GetName(obj.ServerId);
        }
        public Dictionary<long, string> GetServerNames (GetServerNames obj)
        {
            return this._Service.GetNames(obj.ServerId);
        }
        public void DeleteServer (DeleteServer obj)
        {
            this._Service.Delete(obj.ServerId);
        }
        public ServerItem[] GetServerItems (GetServerItems obj)
        {
            return this._Service.GetItems(obj.Query);
        }
        public RemoteServerModel GetServerDatum (GetServerDatum obj)
        {
            return this._Service.GetDatum(obj.Id);
        }
        public RemoteServerDatum GetServer (GetServer obj)
        {
            return this._Service.Get(obj.ServerId);
        }

        public PagingResult<RemoteServer> QueryServer (QueryServer query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }

        public void SetServer (SetServer set)
        {
            this._Service.SetService(set.ServerId, set.Datum);
        }

        public void SetServiceState (SetServiceState set)
        {
            this._Service.SetServiceState(set.ServiceId, set.State);
        }
    }
}
