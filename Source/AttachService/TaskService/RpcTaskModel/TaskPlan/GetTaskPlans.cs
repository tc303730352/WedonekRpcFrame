using WeDonekRpc.Client;
using RpcTaskModel.TaskPlan.Model;

namespace RpcTaskModel.TaskPlan
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class GetTaskPlans : RpcRemoteArray<TaskPlanDatum>
    {
        public long TaskId
        {
            get;
            set;
        }
    }
}
