using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Client;

namespace RpcTaskModel.TaskItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetTaskSelectItem : RpcRemoteArray<TaskSelectItem>
    {
        public long TaskId { get; set; }
    }
}
