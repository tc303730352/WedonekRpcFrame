using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.Collect.lmpl
{

    internal class IdentityAppCollect : IIdentityAppCollect
    {
        private readonly IIdentityAppDAL _BasicDAL;
        public IdentityAppCollect (IIdentityAppDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public long Add (IdentityDatum datum)
        {
            IdentityAppModel add = datum.ConvertMap<IdentityDatum, IdentityAppModel>();
            add.AppId = Guid.NewGuid().ToString("N");
            return this._BasicDAL.Add(add);
        }
        public bool Set (IdentityAppModel app, IdentityDatum param)
        {
            if (param.IsEquals(app))
            {
                return false;
            }
            this._BasicDAL.Set(app.Id, param);
            return true;
        }

        public IdentityApp[] Query (IdentityQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public IdentityAppModel Get (long id)
        {
            IdentityAppModel app = this._BasicDAL.Get(id);
            if (app == null)
            {
                throw new ErrorException("rpc.store.identity.not.find");
            }
            return app;
        }
        public void Delete (IdentityAppModel app)
        {
            if (app.IsEnable)
            {
                throw new ErrorException("rpc.store.identity.already.enable");
            }
            this._BasicDAL.Delete(app.Id);
        }

        public bool SetIsEnable (IdentityAppModel app, bool isEnable)
        {
            if (app.IsEnable == isEnable)
            {
                return false;
            }
            this._BasicDAL.SetIsEnable(app.Id, isEnable);
            return true;
        }
    }
}
