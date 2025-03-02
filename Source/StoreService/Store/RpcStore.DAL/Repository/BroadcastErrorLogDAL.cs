using WeDonekRpc.Model;
using RpcStore.Model.BroadcastErrorLog;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;

namespace RpcStore.DAL.Repository
{
    internal class BroadcastErrorLogDAL : IBroadcastErrorLogDAL
    {
        private readonly IRpcExtendResource<BroadcastErrorLogModel> _BasicDAL;
        public BroadcastErrorLogDAL (IRpcExtendResource<BroadcastErrorLogModel> dal)
        {
            this._BasicDAL = dal;
        }
        public BroadcastErrorLogModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public BroadcastErrorLog[] Query (BroadcastErrorQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<BroadcastErrorLog>(query.ToWhere(), paging, out count);
        }
    }
}
