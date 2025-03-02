using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.ModularModel.Identity.Msg;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Identity.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class IdentityAppService : IIdentityAppService
    {
        private readonly IIdentityAppCollect _Identity;

        public IdentityAppService (IIdentityAppCollect identity)
        {
            this._Identity = identity;
        }

        public long Add (IdentityDatum add)
        {
            return this._Identity.Add(add);
        }

        public void Delete (long id)
        {
            IdentityAppModel app = this._Identity.Get(id);
            this._Identity.Delete(app);
        }

        public IdentityAppData Get (long id)
        {
            IdentityAppModel app = this._Identity.Get(id);
            return app.ConvertMap<IdentityAppModel, IdentityAppData>();
        }

        public PagingResult<IdentityApp> Query (IdentityQuery query, IBasicPage paging)
        {
            IdentityApp[] apps = this._Identity.Query(query, paging, out int count);
            if (apps.IsNull())
            {
                return new PagingResult<IdentityApp>();
            }
            return new PagingResult<IdentityApp>(count, apps);
        }
        public void SetIsEnable (long id, bool isEnable)
        {
            IdentityAppModel app = this._Identity.Get(id);
            if (this._Identity.SetIsEnable(app, isEnable))
            {
                this._Refresh(app.AppId);
            }
        }
        private void _Refresh (string appId)
        {
            new RefreshIdentity()
            {
                AppId = appId
            }.SyncSend();
        }
        public void Set (long id, IdentityDatum param)
        {
            IdentityAppModel app = this._Identity.Get(id);
            if (this._Identity.Set(app, param) && app.IsEnable)
            {
                this._Refresh(app.AppId);
            }
        }
    }
}
