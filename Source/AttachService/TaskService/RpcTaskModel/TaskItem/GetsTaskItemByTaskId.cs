using WeDonekRpc.Client;

namespace RpcTaskModel.TaskItem
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetsTaskItemByTaskId : RpcRemoteArray<Model.TaskItem>
    {
        public long TaskId { get; set; }
    }
}
