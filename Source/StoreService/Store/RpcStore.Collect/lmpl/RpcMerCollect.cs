using WeDonekRpc.Client;
using WeDonekRpc.Helper;

using WeDonekRpc.Model;
using RpcStore.Collect.LocalEvent.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMer;
using RpcStore.RemoteModel.Mer.Model;

namespace RpcStore.Collect.lmpl
{
    internal class RpcMerCollect : IRpcMerCollect
    {
        private readonly IRpcMerDAL _BasicDAL;
        public RpcMerCollect (IRpcMerDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public RpcMerModel GetRpcMer (long id)
        {
            RpcMerModel mer = this._BasicDAL.Get(id);
            if (mer == null)
            {
                throw new ErrorException("rpc.store.mer.not.find");
            }
            return mer;
        }

        public bool SetMer (RpcMerModel mer, RpcMerSetDatum datum)
        {
            if (datum.IsEquals(mer))
            {
                return false;
            }
            else if (mer.SystemName != mer.SystemName && this._BasicDAL.CheckSystemName(datum.SystemName))
            {
                throw new ErrorException("rpc.store.mer.name.repeat");
            }
            this._BasicDAL.Set(mer.Id, datum);
            return true;
        }
        public void Delete (RpcMerModel mer)
        {
            this._BasicDAL.Delete(mer.Id);
            new RpcMerEvent("Delete")
            {
                RpcMer = mer
            }.AsyncPublic();
        }
        public void CheckAppId (string appId)
        {
            if (this._BasicDAL.CheckAppId(appId))
            {
                throw new ErrorException("rpc.store.mer.appId.repeat");
            }
        }
        public void CheckSystemName (string name)
        {
            if (this._BasicDAL.CheckSystemName(name))
            {
                throw new ErrorException("rpc.store.mer.name.repeat");
            }
        }
        public long AddMer (RpcMerAdd mer)
        {
            if (mer.AppId.IsNull())
            {
                mer.AppId = Guid.NewGuid().ToString("N").ToLower();
            }
            if (mer.AppSecret.IsNull())
            {
                mer.AppSecret = Guid.NewGuid().ToString("N").ToLower();
            }
            this.CheckSystemName(mer.SystemName);
            this.CheckAppId(mer.AppId);
            RpcMerModel add = mer.ConvertMap<RpcMerAdd, RpcMerModel>();
            add.Id = this._BasicDAL.Add(add);
            new RpcMerEvent("Add")
            {
                RpcMer = add
            }.AsyncPublic();
            return add.Id;
        }
        public BasicRpcMer[] GetBasic ()
        {
            return this._BasicDAL.GetBasic();
        }
        public BasicRpcMer[] GetBasic (long[] rpcMerId)
        {
            return this._BasicDAL.GetBasic(rpcMerId);
        }
        public string GetName (long rpcMerId)
        {
            return this._BasicDAL.GetName(rpcMerId);
        }
        public RpcMerModel[] Query (string queryKey, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(queryKey, paging, out count);
        }
        public Dictionary<long, string> GetNames (long[] ids)
        {
            if (ids.IsNull())
            {
                return null;
            }
            return this._BasicDAL.GetNames(ids);
        }
    }
}
