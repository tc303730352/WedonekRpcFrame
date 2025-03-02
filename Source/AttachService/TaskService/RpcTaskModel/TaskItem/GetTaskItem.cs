using WeDonekRpc.Client;
using RpcTaskModel.TaskItem.Model;

namespace RpcTaskModel.TaskItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetTaskItem : RpcRemote<TaskItemDatum>
    {
        public long Id { get; set; }
    }
}
