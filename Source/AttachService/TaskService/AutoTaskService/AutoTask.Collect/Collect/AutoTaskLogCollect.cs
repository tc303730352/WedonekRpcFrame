using AutoTask.DAL;
using AutoTask.Model.DB;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcTaskModel.TaskLog.Model;

namespace AutoTask.Collect.Collect
{
    internal class AutoTaskLogCollect : IAutoTaskLogCollect
    {
        private readonly IAutoTaskLogDAL _BasicDAL;


        public AutoTaskLogCollect (IAutoTaskLogDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public TaskLogDatum[] Query (TaskLogQueryParam query, IBasicPage paging, out int count)
        {
            return this._BasicDAL.Query(query, paging, out count);
        }
        public AutoTaskLogModel Get (long id)
        {
            AutoTaskLogModel log = this._BasicDAL.Get(id);
            if (log == null)
            {
                throw new ErrorException("task.log.not.find");
            }
            return log;
        }
        public void Adds (AutoTaskLogModel[] logs)
        {
            this._BasicDAL.Adds(logs);
        }

    }
}
