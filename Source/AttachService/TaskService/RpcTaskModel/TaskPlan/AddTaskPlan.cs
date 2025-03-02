using WeDonekRpc.Client;

namespace RpcTaskModel.TaskPlan
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class AddTaskPlan : RpcRemote<long>
    {
        public Model.TaskPlanAdd TaskPlan { get; set; }
    }
}
