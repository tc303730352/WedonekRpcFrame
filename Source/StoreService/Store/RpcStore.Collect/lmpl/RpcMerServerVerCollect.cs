using RpcStore.DAL;
using RpcStore.Model.DB;

namespace RpcStore.Collect.lmpl
{
    internal class RpcMerServerVerCollect : IRpcMerServerVerCollect
    {
        private readonly IRpcMerServerVerDAL _BasicDAL;

        public RpcMerServerVerCollect (IRpcMerServerVerDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public void Add (long rpcMerId, long systemTypeId, int verNum)
        {
            this._BasicDAL.Add(new RpcMerServerVerModel
            {
                SystemTypeId = systemTypeId,
                CurrentVer = verNum,
                RpcMerId = rpcMerId
            });
        }

        public RpcMerServerVerModel Find (long rpcMerId, long systemTypeId)
        {
            return this._BasicDAL.Find(rpcMerId, systemTypeId);
        }

        public Dictionary<long, int> GetVers (long rpcMerId)
        {
            return this._BasicDAL.GetVers(rpcMerId);
        }

        public Dictionary<long, int> GetVers (long[] rpcMerId, long[] systemTypeId)
        {
            return this._BasicDAL.GetVers(rpcMerId, systemTypeId);
        }
        public void SetCurrentVer (RpcMerServerVerModel source, int ver)
        {
            if (source.CurrentVer == ver)
            {
                return;
            }
            this._BasicDAL.SetCurrentVer(source.Id, ver);
        }
    }
}
