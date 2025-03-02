using WeDonekRpc.Client;
using RpcTaskModel.TaskLog.Model;

namespace RpcTaskModel.TaskLog
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class QueryTaskLog : BasicPage<TaskLogDatum>
    {
        public TaskLogQueryParam Query
        {
            get;
            set;
        }
    }
}
