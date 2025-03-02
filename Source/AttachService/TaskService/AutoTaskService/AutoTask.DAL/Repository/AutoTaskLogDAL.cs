using AutoTask.Model.DB;
using RpcTaskModel.TaskLog.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace AutoTask.DAL.Repository
{
    internal class AutoTaskLogDAL : IAutoTaskLogDAL
    {
        private readonly IRepository<AutoTaskLogModel> _BasicDAL;
        public AutoTaskLogDAL (IRepository<AutoTaskLogModel> dal)
        {
            this._BasicDAL = dal;
        }
        public TaskLogDatum[] Query (TaskLogQueryParam query, IBasicPage paging, out int count)
        {
            paging.InitOrderBy("Id", true);
            return this._BasicDAL.Query<TaskLogDatum>(query.ToWhere(), paging, out count);
        }
        public void Adds (AutoTaskLogModel[] logs)
        {
            logs.ForEach(a =>
            {
                a.Id = IdentityHelper.CreateId();
            });
            this._BasicDAL.Insert(logs);
        }

        public AutoTaskLogModel Get (long id)
        {
            return this._BasicDAL.Get(a => a.Id == id);
        }
    }
}
