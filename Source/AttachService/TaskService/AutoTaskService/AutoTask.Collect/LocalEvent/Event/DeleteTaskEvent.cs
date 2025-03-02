using AutoTask.DAL;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace AutoTask.Collect.LocalEvent.Event
{
    [LocalEventName("delete")]
    internal class DeleteTaskEvent : IEventHandler<AutoTaskEvent>
    {
        private readonly IAutoTaskItemDAL _TaskItem;
        private readonly IAutoTaskPlanDAL _TaskPlan;
        public DeleteTaskEvent (IAutoTaskItemDAL taskItem, IAutoTaskPlanDAL taskPlan)
        {
            this._TaskItem = taskItem;
            this._TaskPlan = taskPlan;
        }

        public void HandleEvent (AutoTaskEvent data, string eventName)
        {
            this._TaskItem.Clear(data.TaskId);
            this._TaskPlan.Clear(data.TaskId);
        }
    }
}
