using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ServerType;
using RpcStore.RemoteModel.ServerType.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    public class ServerTypeEvent : IRpcApiService
    {
        private readonly IServerTypeService _Service;
        public ServerTypeEvent (IServerTypeService service)
        {
            this._Service = service;
        }
        public long AddServerType (AddServerType add)
        {
            return this._Service.Add(add.Datum);
        }
        public string GetServerTypeNameByTypeVal(GetServerTypeNameByTypeVal obj)
        {
            return this._Service.GetName(obj.TypeVal);
        }
        public void CheckServerTypeVal (CheckServerTypeVal obj)
        {
            this._Service.CheckIsRepeat(obj.TypeVal);
        }

        public void DeleteServerType (DeleteServerType obj)
        {
            this._Service.Delete(obj.Id);
        }

        public ServerType GetServerType (GetServerType obj)
        {
            return this._Service.Get(obj.Id);
        }

        public ServerType[] GetServerTypeByGroup (GetServerTypeByGroup obj)
        {
            return this._Service.Gets(obj.GroupId);
        }

        public PagingResult<ServerTypeDatum> QueryServerType (QueryServerType query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }

        public void SetServerType (SetServerType param)
        {
            this._Service.Set(param.Id, param.Datum);
        }
    }
}
