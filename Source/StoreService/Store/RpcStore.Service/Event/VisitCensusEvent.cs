using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.VisitCensus;
using RpcStore.RemoteModel.VisitCensus.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class VisitCensusEvent : IRpcApiService
    {
        private IServerVisitCensusService _Service;

        public VisitCensusEvent(IServerVisitCensusService service)
        {
            _Service = service;
        }

        public PagingResult<ServerVisitCensus> QueryVisitCensus(QueryVisitCensus query)
        {
            return _Service.Query(query.Query, query.ToBasicPage());
        }

        public void ResetVisitCensus(ResetVisitCensus obj)
        {
            _Service.Reset(obj.ServerId);
        }

        public void SetVisitCensusShow(SetVisitCensusShow set)
        {
            _Service.SetShow(set.Id, set.Show);
        }
    }
}
