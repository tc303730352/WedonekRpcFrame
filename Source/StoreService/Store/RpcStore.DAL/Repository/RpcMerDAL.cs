using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Helper.Validate;
using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMer;
using RpcStore.RemoteModel.Mer.Model;
using WeDonekRpc.SqlSugar;
namespace RpcStore.DAL.Repository
{
    internal class RpcMerDAL : IRpcMerDAL
    {
        private readonly IRepository<RpcMerModel> _BasicDAL;
        public RpcMerDAL (IRepository<RpcMerModel> dal)
        {
            this._BasicDAL = dal;
        }

        public void Set (long id, RpcMerSetDatum datum)
        {
            if (!this._BasicDAL.Update(datum, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.mer.set.error");
            }
        }
        public long Add (RpcMerModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public bool CheckAppId (string appId)
        {
            return this._BasicDAL.IsExist(c => c.AppId == appId);
        }
        public bool CheckSystemName (string sysName)
        {
            return this._BasicDAL.IsExist(c => c.SystemName == sysName);
        }
        public RpcMerModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.mer.delete.error");
            }
        }
        public Dictionary<long, string> GetNames (long[] ids)
        {
            BasicRpcMer[] mers = this._BasicDAL.Gets(c => ids.Contains(c.Id), c => new BasicRpcMer
            {
                Id = c.Id,
                SystemName = c.SystemName
            });
            return mers.ToDictionary(c => c.Id, c => c.SystemName);
        }
        public RpcMerModel[] Query (string queryKey, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            if (queryKey.IsNull())
            {
                return this._BasicDAL.Query(paging, out count);
            }
            else if (queryKey.Validate(ValidateFormat.Guid))
            {
                return this._BasicDAL.Query(a => a.AppId == queryKey, paging, out count);
            }
            return this._BasicDAL.Query(a => a.SystemName.Contains(queryKey), paging, out count);
        }


        public BasicRpcMer[] GetBasic ()
        {
            return this._BasicDAL.GetAll<BasicRpcMer>();
        }
        public BasicRpcMer[] GetBasic (long[] rpcMerId)
        {
            return this._BasicDAL.Gets<BasicRpcMer>(a => rpcMerId.Contains(a.Id));
        }

        public string GetName (long rpcMerId)
        {
            return this._BasicDAL.Get(a => a.Id == rpcMerId, a => a.SystemName);
        }
    }
}
