using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.VisitCensus.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class ServerVisitCensusService : IServerVisitCensusService
    {
        private readonly IServerVisitCensusCollect _VisitCensus;
        public ServerVisitCensusService (IServerVisitCensusCollect visit)
        {
            this._VisitCensus = visit;
        }
        public PagingResult<ServerVisitCensus> Query (VisitCensusQuery query, IBasicPage paging)
        {
            ServerVisitCensusModel[] list = this._VisitCensus.Query(query, paging, out int count);
            if (list.IsNull())
            {
                return new PagingResult<ServerVisitCensus>();
            }
            ServerVisitCensus[] census = list.ConvertMap<ServerVisitCensusModel, ServerVisitCensus>();
            return new PagingResult<ServerVisitCensus>(count, census);
        }

        public void Reset (long serverId)
        {
            this._VisitCensus.Reset(serverId);
        }

        public void SetShow (long id, string show)
        {
            this._VisitCensus.SetShow(id, show);
        }
    }
}
