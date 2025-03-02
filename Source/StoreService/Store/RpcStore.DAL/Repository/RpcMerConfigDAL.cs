using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using RpcStore.Model.DB;
using RpcStore.Model.RpcMerConfig;
using RpcStore.RemoteModel.MerConfig.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class RpcMerConfigDAL : IRpcMerConfigDAL
    {
        private readonly IRepository<RpcMerConfigModel> _BasicDAL;
        public RpcMerConfigDAL (IRepository<RpcMerConfigModel> dal)
        {
            this._BasicDAL = dal;
        }
        public bool CheckIsExists (long rpcMerId, long sysTypeId)
        {
            return this._BasicDAL.IsExist(c => c.RpcMerId == rpcMerId && c.SystemTypeId == sysTypeId);
        }
        public RpcMerConfigModel[] Gets (long rpcMerId)
        {
            return this._BasicDAL.Gets(c => c.RpcMerId == rpcMerId);
        }
        public RpcMerConfigModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }

        public void Add (RpcMerConfigModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
        }

        public void Set (long id, MerConfigSet config)
        {
            if (!this._BasicDAL.Update(config, c => c.Id == id))
            {
                throw new ErrorException("rpc.store.mer.config.set.error");
            }
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("rpc.store.mer.config.delete.error");
            }
        }

        public RpcMerConfig Get (long rpcMerId, long systemTypeId)
        {
            return this._BasicDAL.Get<RpcMerConfig>(a => a.RpcMerId == rpcMerId && a.SystemTypeId == systemTypeId);
        }
    }
}
