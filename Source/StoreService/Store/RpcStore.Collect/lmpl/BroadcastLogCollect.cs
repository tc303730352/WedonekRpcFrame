using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.BroadcastErrorLog;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;

namespace RpcStore.Collect.lmpl
{
    internal class BroadcastLogCollect : IBroadcastLogCollect
    {
        private readonly IBroadcastErrorLogDAL _BasicDAL;

        public BroadcastLogCollect (IBroadcastErrorLogDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }

        public BroadcastErrorLog[] Query (BroadcastErrorQuery query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }

        public BroadcastErrorLogModel Get (long id)
        {
            return this._BasicDAL.Get(id);
        }
    }
}
