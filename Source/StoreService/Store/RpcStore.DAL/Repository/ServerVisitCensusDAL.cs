using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.DAL;
using RpcStore.Model.DB;
using RpcStore.RemoteModel.VisitCensus.Model;
using WeDonekRpc.SqlSugar;

namespace RpcStore.DAL.Repository
{
    internal class ServerVisitCensusDAL : IServerVisitCensusDAL
    {
        private readonly IRepository<ServerVisitCensusModel> _BasicDAL;
        public ServerVisitCensusDAL ( IRepository<ServerVisitCensusModel> dal )
        {
            this._BasicDAL = dal;
        }

        public void Clear ( long serverId )
        {
            _ = this._BasicDAL.Delete(a => a.ServiceId == serverId);
        }
        public ServerVisitCensusModel[] Query ( VisitCensusQuery query, IBasicPage paging, out int count )
        {
            paging.InitOrderBy("Id", false);
            return this._BasicDAL.Query(query.ToWhere(), paging, out count);
        }

        public void SetShow ( long id, string show )
        {
            if ( !this._BasicDAL.Update(a => a.Show == show, a => a.Id == id) )
            {
                throw new ErrorException("rpc.store.visit.census.show.set.fail");
            }
        }

        public void Reset ( long serverId )
        {
            _ = this._BasicDAL.Update(a => a.VisitNum == 0 &&
            a.SuccessNum == 0 &&
            a.FailNum == 0 &&
            a.TodayFail == 0 &&
            a.TodaySuccess == 0 &&
            a.TodayVisit == 0, a => a.ServiceId == serverId);
        }
    }
}
