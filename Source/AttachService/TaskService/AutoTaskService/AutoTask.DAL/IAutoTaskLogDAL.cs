using AutoTask.Model.DB;
using WeDonekRpc.Model;
using RpcTaskModel.TaskLog.Model;

namespace AutoTask.DAL
{
    public interface IAutoTaskLogDAL
    {
        TaskLogDatum[] Query(TaskLogQueryParam query, IBasicPage paging, out int count);
        void Adds(AutoTaskLogModel[] logs);
        AutoTaskLogModel Get(long id);
    }
}