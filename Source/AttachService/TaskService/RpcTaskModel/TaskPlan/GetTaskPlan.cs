using WeDonekRpc.Client;

namespace RpcTaskModel.TaskPlan
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetTaskPlan : RpcRemote<Model.TaskPlanDatum>
    {
        public long Id { get; set; }
    }
}
