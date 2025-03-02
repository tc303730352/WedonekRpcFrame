using WeDonekRpc.Model;
using RpcStore.RemoteModel.VisitCensus;
using RpcStore.RemoteModel.VisitCensus.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class VisitCensusService : IVisitCensusService
    {
        public ServerVisitCensus[] QueryVisitCensus(VisitCensusQuery query, IBasicPage paging, out int count)
        {
            return new QueryVisitCensus
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

        public void ResetVisitCensus(long serverId)
        {
            new ResetVisitCensus
            {
                ServerId = serverId,
            }.Send();
        }

        public void SetVisitCensusShow(long id, string show)
        {
            new SetVisitCensusShow
            {
                Id = id,
                Show = show,
            }.Send();
        }

    }
}
