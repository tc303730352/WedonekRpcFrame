using WeDonekRpc.Model;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Trace.Model;

namespace RpcStore.DAL.Repository
{
    internal class RpcTraceListDAL : IRpcTraceListDAL
    {
        private readonly IRpcExtendResource<RpcTraceModel> _BasicDAL;
        public RpcTraceListDAL (IRpcExtendResource<RpcTraceModel> dal)
        {
            this._BasicDAL = dal;
        }

        public RpcTraceModel[] Query (TraceQuery query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }

    }
}
