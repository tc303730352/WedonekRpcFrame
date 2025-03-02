using DataBasicCli.RpcExtendService;

namespace DataBasicCli.Model
{
    internal class RpcExtendServiceTemplate
    {
        public AutoTaskItemModel[] TaskItem { get; set; }
        public AutoTaskModel[] Task { get; set; }
        public AutoTaskPlanModel[] TaskPlan { get; set; }
        public SystemEventConfigModel[] EventConfig { get; set; }
        public ServerEventSwitchModel[] EventSwitch { get; set; }
    }
}
