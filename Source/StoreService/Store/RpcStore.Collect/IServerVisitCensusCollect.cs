using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.VisitCensus.Model;

namespace RpcStore.Collect
{
    public interface IServerVisitCensusCollect
    {
        ServerVisitCensusModel[] Query(VisitCensusQuery query, IBasicPage paging, out int count);
        void Reset(long serverId);
        void SetShow(long id, string remark);
    }
}