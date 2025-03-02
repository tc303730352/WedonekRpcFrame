using WeDonekRpc.Client;

namespace RpcTaskModel.TaskPlan
{
    [WeDonekRpc.Model.IRemoteConfig("sys.task")]
    public class SetTaskPlanIsEnable : RpcRemote<bool>
    {
        public long Id { get; set; }

        public bool IsEnable { get; set; }
    }
}
