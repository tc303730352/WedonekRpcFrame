using AutoTask.Model.DB;
using WeDonekRpc.Model;
using RpcTaskModel.TaskLog.Model;

namespace AutoTask.Collect
{
    public interface IAutoTaskLogCollect
    {
        AutoTaskLogModel Get (long id);
        TaskLogDatum[] Query (TaskLogQueryParam query, IBasicPage paging, out int count);
        void Adds (AutoTaskLogModel[] logs);
    }
}