using WeDonekRpc.Helper;
using RpcStore.Model.ExtendDB;

namespace RpcStore.DAL.Repository
{
    internal class DictateNodeRelationDAL : IDictateNodeRelationDAL
    {
        private readonly IRpcExtendResource<DictateNodeRelationModel> _BasicDAL;
        public DictateNodeRelationDAL (IRpcExtendResource<DictateNodeRelationModel> dal)
        {
            this._BasicDAL = dal;
        }

        public long[] GetParents (long subId)
        {
            return this._BasicDAL.Gets(c => c.SubId == subId, c => c.ParentId);
        }
        public long[] GetSubs (long parentId)
        {
            return this._BasicDAL.Gets(c => c.ParentId == parentId, c => c.SubId);
        }
        public void Clear (long subId)
        {
            if (!this._BasicDAL.Delete(a => a.SubId == subId))
            {
                throw new ErrorException("rpc.store.dictate.relation.drop.error");
            }
        }
        public void Delete (long[] subId)
        {
            _ = this._BasicDAL.Delete(a => subId.Contains(a.SubId));
        }

        public void Add (long subId, long[] parentId)
        {
            this._BasicDAL.Insert(parentId.ConvertAll(c => new DictateNodeRelationModel
            {
                SubId = subId,
                ParentId = c
            }));
        }
    }
}
