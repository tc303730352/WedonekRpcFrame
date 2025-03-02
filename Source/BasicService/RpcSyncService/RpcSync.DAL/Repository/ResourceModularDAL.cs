using WeDonekRpc.Helper.IdGenerator;
using RpcSync.Model.DB;

namespace RpcSync.DAL.Repository
{
    internal class ResourceModularDAL : IResourceModularDAL
    {
        private readonly IRpcExtendResource<ResourceModularModel> _BasicDAL;
        public ResourceModularDAL(IRpcExtendResource<ResourceModularModel> dal)
        {
            this._BasicDAL = dal;
        }

        public long AddModular(ResourceModularModel add)
        {
            add.Id = IdentityHelper.CreateId();
            add.AddTime = DateTime.Now;
            this._BasicDAL.Insert(add);
            return add.Id;
        }

        public long FindModular(string key)
        {
            return this._BasicDAL.Get(c => c.ModularKey == key, c => c.Id);
        }

    }
}
