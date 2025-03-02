using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.ReduceInRank.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ReduceInRankConfigDAL : IReduceInRankConfigDAL
    {
        private readonly IRepository<ReduceInRankConfigModel> _BasicDAL;
        public ReduceInRankConfigDAL (IRepository<ReduceInRankConfigModel> dal)
        {
            this._BasicDAL = dal;
        }
        public ReduceInRankConfigModel Get (long rpcMerId, long serverId)
        {
            return this._BasicDAL.Get(c => c.RpcMerId == rpcMerId && c.ServerId == serverId);
        }
        public void Clear (long rpcMerId, long serverId)
        {
            _ = this._BasicDAL.Delete(c => c.RpcMerId == rpcMerId && c.ServerId == serverId);
        }
        public ReduceInRankConfigModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }

        public void Set (long id, ReduceInRankDatum datum)
        {
            if (!this._BasicDAL.Update(datum, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.reduceInRank.set.error");
            }
        }
        public long Add (ReduceInRankConfigModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public void Clear (long serverId)
        {
            _ = this._BasicDAL.Delete(c => c.ServerId == serverId);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.reduceInRank.drop.error");
            }
        }
    }
}
