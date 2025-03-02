using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.Identity;
using RpcStore.RemoteModel.Identity.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class IdentityAppEvent : IRpcApiService
    {
        private readonly IIdentityAppService _Service;

        public IdentityAppEvent (IIdentityAppService service)
        {
            this._Service = service;
        }

        public long AddIdentityApp (AddIdentityApp add)
        {
            return this._Service.Add(add.Datum);
        }

        public void DeleteIdentityApp (DeleteIdentityApp obj)
        {
            this._Service.Delete(obj.Id);
        }

        public IdentityAppData GetIdentityApp (GetIdentityApp obj)
        {
            return this._Service.Get(obj.Id);
        }

        public PagingResult<IdentityApp> QueryIdentity (QueryIdentity query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }

        public void SetIdentity (SetIdentity obj)
        {
            this._Service.Set(obj.Id, obj.Datum);
        }

        public void SetIdentityIsEnable (SetIdentityIsEnable obj)
        {
            this._Service.SetIsEnable(obj.Id, obj.IsEnable);
        }
    }
}
