using WeDonekRpc.Client;
using WeDonekRpc.Model;
using RpcStore.RemoteModel.VisitCensus.Model;

namespace RpcStore.Service.Interface
{
    public interface IServerVisitCensusService
    {
        PagingResult<ServerVisitCensus> Query(VisitCensusQuery query, IBasicPage paging);
        void Reset(long serverId);
        void SetShow(long id, string show);
    }
}