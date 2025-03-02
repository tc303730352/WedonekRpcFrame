using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcStore.DAL;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.DictateNode.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.Collect.lmpl
{
    internal class DictateNodeCollect : IDictateNodeCollect
    {
        private readonly IDictateNodeDAL _BasicDAL;

        private readonly IDictateNodeRelationDAL _RelationDAL = null;

        private readonly ITransactionService _Tran;
        public DictateNodeCollect (IDictateNodeDAL basicDAL,
            ITransactionService tran,
            IDictateNodeRelationDAL relationDAL)
        {
            this._Tran = tran;
            this._BasicDAL = basicDAL;
            this._RelationDAL = relationDAL;
        }

        public long Add (DictateNodeAdd datum)
        {
            DictateNodeModel add = datum.ConvertMap<DictateNodeAdd, DictateNodeModel>();
            if (add.ParentId == 0)
            {
                return this._BasicDAL.Add(add);
            }
            long[] ids = this._RelationDAL.GetParents(add.ParentId);
            ids = ids.Add(add.ParentId);
            using (ILocalTransaction tran = this._Tran.ApplyTran())
            {
                long id = this._BasicDAL.Add(add);
                this._RelationDAL.Add(id, ids);
                tran.Commit();
                return id;
            }

        }
        public void Delete (DictateNodeModel obj)
        {
            long[] subId = this._RelationDAL.GetSubs(obj.Id);
            subId = subId.Add(obj.Id);
            using (ILocalTransaction tran = this._Tran.ApplyTran())
            {
                this._BasicDAL.Delete(subId);
                this._RelationDAL.Delete(subId);
                tran.Commit();
            }
        }
        public DictateNodeModel[] Gets ()
        {
            return this._BasicDAL.Gets();
        }
        public DictateNodeModel Get (long id)
        {
            DictateNodeModel node = this._BasicDAL.Get(id);
            if (node == null)
            {
                throw new ErrorException("rpc.store.dictate.node.not.find");
            }
            return node;
        }
        public DictateNodeModel[] Gets (long parentId)
        {
            long[] ids = this._RelationDAL.GetSubs(parentId);
            return this._BasicDAL.Gets(ids);
        }
        public void Set (DictateNodeModel node, string name)
        {
            if (node.DictateName == name)
            {
                return;
            }
            this._BasicDAL.Set(node.Id, name);
        }
    }
}
