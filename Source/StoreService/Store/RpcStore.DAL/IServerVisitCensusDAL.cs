using WeDonekRpc.Model;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.VisitCensus.Model;

namespace RpcStore.DAL
{
    public interface IServerVisitCensusDAL
    {
        void Clear(long serverId);
        ServerVisitCensusModel[] Query(VisitCensusQuery query, IBasicPage paging, out int count);
        void Reset(long serverId);
        void SetShow(long id, string show);
    }
}