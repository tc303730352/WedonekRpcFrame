using RpcStore.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class RpcMerServerVerDAL : IRpcMerServerVerDAL
    {
        private readonly IRepository<RpcMerServerVerModel> _BasicDAL;
        public RpcMerServerVerDAL (IRepository<RpcMerServerVerModel> dal)
        {
            this._BasicDAL = dal;
        }
        public Dictionary<long, int> GetVers (long rpcMerId)
        {
            return this._BasicDAL.Gets(a => a.RpcMerId == rpcMerId, c => new
            {
                c.SystemTypeId,
                c.CurrentVer
            }).ToDictionary(c => c.SystemTypeId, c => c.CurrentVer);
        }
        public Dictionary<long, int> GetVers (long[] rpcMerId, long[] systemTypeId)
        {
            return this._BasicDAL.Gets(a => rpcMerId.Contains(a.RpcMerId) && systemTypeId.Contains(a.SystemTypeId), c => new
            {
                c.SystemTypeId,
                c.CurrentVer
            }).ToDictionary(c => c.SystemTypeId, c => c.CurrentVer);
        }
        public RpcMerServerVerModel Find (long rpcMerId, long systemTypeId)
        {
            return this._BasicDAL.Get(a => a.RpcMerId == rpcMerId && a.SystemTypeId == systemTypeId);
        }
        public void SetCurrentVer (long id, int ver)
        {
            if (!this._BasicDAL.Update(a => a.CurrentVer == ver, a => a.Id == id))
            {
                throw new ErrorException("rpc.store.ver.set.fail");
            }
        }

        public void Add (RpcMerServerVerModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
        }

        public int? GetVer (long rpcMerId, long systemTypeId)
        {
            var ver = this._BasicDAL.Get(a => a.RpcMerId == rpcMerId && a.SystemTypeId == systemTypeId, a => new
            {
                ver = a.CurrentVer
            });
            if (ver == null)
            {
                return null;
            }
            return ver.ver;
        }
    }
}
