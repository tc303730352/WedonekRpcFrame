using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Identity.Model;

namespace RpcStore.DAL.Repository
{
    internal class IdentityAppDAL : IIdentityAppDAL
    {
        private readonly IRpcExtendResource<IdentityAppModel> _BasicDAL;
        public IdentityAppDAL (IRpcExtendResource<IdentityAppModel> dal)
        {
            this._BasicDAL = dal;
        }
        public long Add (IdentityAppModel add)
        {
            add.Id = IdentityHelper.CreateId();
            add.CreateTime = DateTime.Now;
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public void SetIsEnable (long id, bool isEnable)
        {
            if (!this._BasicDAL.Update(a => a.IsEnable == isEnable, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.route.set.fail");
            }
        }
        public void Set (long id, IdentityDatum param)
        {
            if (!this._BasicDAL.Update(param, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.identity.set.fail");
            }
        }
        public IdentityApp[] Query (IdentityQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<IdentityApp>(query.ToWhere(), paging, out count);
        }
        public IdentityAppModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.identity.delete.fail");
            }
        }

    }
}
