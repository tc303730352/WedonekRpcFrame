using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.ServerPublic;
using RpcStore.RemoteModel.ServerPublic.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class PublicSchemeEvent : IRpcApiService
    {
        private readonly IPublicSchemeService _Service;

        public PublicSchemeEvent (IPublicSchemeService service)
        {
            this._Service = service;
        }
        public bool EnablePublicScheme (EnablePublicScheme obj)
        {
            return this._Service.Enable(obj.Id);
        }
        public bool DisablePublicScheme (DisablePublicScheme obj)
        {
            return this._Service.Disable(obj.Id);
        }
        public long AddPublicScheme (AddPublicScheme add)
        {
            return this._Service.Add(add.SchemeAdd);
        }
        public void DeletePublicScheme (DeletePublicScheme obj)
        {
            this._Service.Delete(obj.Id);
        }
        public PublicScheme GetPublicScheme (GetPublicScheme obj)
        {
            return this._Service.Get(obj.Id);
        }
        public void SetPublicScheme (SetPublicScheme obj)
        {
            this._Service.Set(obj.Id, obj.Scheme);
        }
        public PagingResult<ServerPublicScheme> QueryPublicScheme (QueryPublicScheme query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }
    }
}
