using AutoTask.Collect;
using AutoTask.Model.DB;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcTaskModel.TaskLog;
using RpcTaskModel.TaskLog.Model;

namespace AutoTask.Service.Event
{
    internal class TaskLogEvent : IRpcApiService
    {
        private readonly IAutoTaskLogCollect _TaskLog;
        public TaskLogEvent (IAutoTaskLogCollect taskLog)
        {
            this._TaskLog = taskLog;
        }
        public TaskLogDetailed GetTaskLog (GetTaskLog obj)
        {
            AutoTaskLogModel log = this._TaskLog.Get(obj.Id);
            return log.ConvertMap<AutoTaskLogModel, TaskLogDetailed>();
        }
        public PagingResult<TaskLogDatum> QueryTaskLog (QueryTaskLog query)
        {
            TaskLogDatum[] logs = this._TaskLog.Query(query.Query, query.ToBasicPage(), out int count);
            return new PagingResult<TaskLogDatum>(logs, count);
        }
    }
}
