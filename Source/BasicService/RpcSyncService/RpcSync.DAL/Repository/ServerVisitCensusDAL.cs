using RpcSync.Model;
using RpcSync.Model.DB;
using SqlSugar;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace RpcSync.DAL.Repository
{
    internal class ServerVisitCensusDAL : IServerVisitCensusDAL
    {
        private readonly IRepository<ServerVisitCensusModel> _BasicDAL;
        public ServerVisitCensusDAL (IRepository<ServerVisitCensusModel> dal)
        {
            this._BasicDAL = dal;
        }
        public void Add (ServerVisitCensusModel add)
        {
            add.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(add);
        }
        public bool CheckIsExists (long serverId, string dictate)
        {
            return this._BasicDAL.IsExist(a => a.ServiceId == serverId && a.Dictate == dictate);
        }
        public void Adds (ServerVisitCensusModel[] adds)
        {
            _ = this._BasicDAL.Insert(new List<ServerVisitCensusModel>(adds), a => new
            {
                a.ServiceId,
                a.Dictate
            });
        }
        public void Clear ()
        {
            _ = this._BasicDAL.Update(a => new ServerVisitCensusModel
            {
                TodayFail = 0,
                TodaySuccess = 0,
                TodayVisit = 0
            }, a => 1 == 1);
        }
        public void Sync (RpcVisitCensus[] list)
        {
            ISqlQueue<ServerVisitCensusModel> queue = this._BasicDAL.BeginQueue();
            // string[] wheres = new string[] { "ServiceId", "Dictate" };
            list.ForEach(c =>
            {
                int visit = c.Success + c.Fail;
                IUpdateable<ServerVisitCensusModel> update = queue.Update();
                update = update.Where(a => a.ServiceId == c.ServiceId && a.Dictate == c.Dictate);
                update = update.SetColumns(a => a.VisitNum == a.VisitNum + visit);
                update = update.SetColumns(a => a.TodayVisit == a.TodayVisit + visit);
                if (c.Success != 0)
                {
                    update = update.SetColumns(a => a.SuccessNum == a.SuccessNum + c.Success);
                    update = update.SetColumns(a => a.TodaySuccess == a.TodaySuccess + c.Success);
                }
                if (c.Fail != 0)
                {
                    update = update.SetColumns(a => a.FailNum == a.FailNum + c.Fail);
                    update = update.SetColumns(a => a.TodayFail == a.TodayFail + c.Fail);
                }
                update.AddQueue();
            });
            _ = queue.Submit(false);
        }
    }
}
