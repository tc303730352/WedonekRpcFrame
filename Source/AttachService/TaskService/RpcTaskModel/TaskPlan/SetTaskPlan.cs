using WeDonekRpc.Client;
using RpcTaskModel.TaskPlan.Model;

namespace RpcTaskModel.TaskPlan
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class SetTaskPlan : RpcRemote<bool>
    {
        public long Id
        {
            get;
            set;
        }
        public TaskPlanSet TaskPlan
        {
            get;
            set;
        }
    }
}
