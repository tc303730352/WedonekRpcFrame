using WeDonekRpc.Client;

namespace RpcTaskModel.TaskPlan
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class DeleteTaskPlan : RpcRemote
    {
        public long Id { get; set; }
    }
}
