using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.ModularModel.Visit.Model;
using RpcSync.DAL;
using RpcSync.Model;
using RpcSync.Model.DB;

namespace RpcSync.Collect.Collect
{
    internal class DicateVisitCollect : IDicateVisitCollect
    {
        private readonly IServerVisitCensusDAL _ServerVisit;
        private readonly IVisitCensusDelaySave _CensusLog;

        public DicateVisitCollect(IServerVisitCensusDAL serverVisit, IVisitCensusDelaySave censusLog)
        {
            this._ServerVisit = serverVisit;
            this._CensusLog = censusLog;
        }

        public void AddNode(RpcVisit visit, long serverId)
        {
            if (!this._ServerVisit.CheckIsExists(serverId, visit.Dictate))
            {
                this._ServerVisit.Add(new ServerVisitCensusModel
                {
                    Dictate = visit.Dictate,
                    ServiceId = serverId,
                    Show = visit.Show
                });
            }
        }

        public void ClearVisit()
        {
            this._ServerVisit.Clear();
        }

        public void AddNode(RpcVisit[] visits, long serverId)
        {
            ServerVisitCensusModel[] list = visits.ConvertMap<RpcVisit, ServerVisitCensusModel>((a, b) =>
            {
                b.Id = IdentityHelper.CreateId();
                b.ServiceId = serverId;
                if(b.Show == null)
                {
                    b.Show = string.Empty;
                }
                return b;
            });
            this._ServerVisit.Adds(list);
        }
        public void AddLog(RpcDictateVisit[] adds, long serverId)
        {
            adds.ForEach(c =>
            {
                this._CensusLog.AddLog(new RpcVisitCensus
                {
                    Concurrent = c.Concurrent,
                    Fail = c.Failure,
                    Dictate = c.Dictate,
                    ServiceId = serverId,
                    Success = c.Success
                });
            });
        }
    }
}
