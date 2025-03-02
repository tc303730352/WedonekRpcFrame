using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.VisitCensus.Model;

namespace RpcStore.Collect.lmpl
{
    internal class ServerVisitCensusCollect : IServerVisitCensusCollect
    {
        private IServerVisitCensusDAL _BasicDAL;

        public ServerVisitCensusCollect(IServerVisitCensusDAL basicDAL)
        {
            _BasicDAL = basicDAL;
        }
        public ServerVisitCensusModel[] Query(VisitCensusQuery query, IBasicPage paging, out int count)
        {
            return _BasicDAL.Query(query, paging, out count);
        }
        public void SetShow(long id, string remark)
        {
            _BasicDAL.SetShow(id, remark);
        }
        public void Reset(long serverId)
        {
            _BasicDAL.Reset(serverId);
        }
    }
}
