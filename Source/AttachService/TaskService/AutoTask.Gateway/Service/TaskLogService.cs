using AutoTask.Gateway.Interface;
using RpcTaskModel.TaskLog;
using RpcTaskModel.TaskLog.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace AutoTask.Gateway.Service
{
    internal class TaskLogService : ITaskLogService
    {
        public PagingResult<TaskLogDatum> Query (TaskLogQueryParam query, IBasicPage paging)
        {
            return new QueryTaskLog
            {
                Index = paging.Index,
                IsDesc = paging.IsDesc,
                Size = paging.Size,
                SortName = paging.SortName,
                NextId = paging.NextId,
                Query = query,
            }.Send();
        }
        public TaskLogDetailed Get (long id)
        {
            return new GetTaskLog
            {
                Id = id
            }.Send();
        }
    }
}
