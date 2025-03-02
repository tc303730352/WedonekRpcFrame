using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using RpcStore.Model.ExtendDB;

namespace RpcStore.DAL.Repository
{
    internal class DictateNodeDAL : IDictateNodeDAL
    {
        private readonly IRpcExtendResource<DictateNodeModel> _BasicDAL;
        public DictateNodeDAL (IRpcExtendResource<DictateNodeModel> dal)
        {
            this._BasicDAL = dal;
        }

        public long Add (DictateNodeModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
            return add.Id;
        }
        public void Delete (long[] ids)
        {
            _ = this._BasicDAL.Delete(c => ids.Contains(c.Id));
        }
        public DictateNodeModel[] Gets ()
        {
            return this._BasicDAL.GetAll();
        }
        public DictateNodeModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public DictateNodeModel[] Gets (long[] ids)
        {
            return this._BasicDAL.Gets(c => ids.Contains(c.Id));
        }
        public void Set (long id, string name)
        {
            if (!this._BasicDAL.Update(c => c.DictateName == name, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.dictate.node.set.fail");
            }
        }
    }
}
