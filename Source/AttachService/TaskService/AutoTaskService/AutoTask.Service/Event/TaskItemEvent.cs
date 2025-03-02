using RpcTaskModel.TaskItem;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Client.Interface;

namespace AutoTask.Service.Event
{
    internal class TaskItemEvent : IRpcApiService
    {
        private readonly ITaskItemService _Service;

        public TaskItemEvent (ITaskItemService service)
        {
            this._Service = service;
        }
        public TaskSelectItem[] GetTaskSelectItem (GetTaskSelectItem obj)
        {
            return this._Service.GetSelectItems(obj.TaskId);
        }
        public long AddTaskItem (AddTaskItem add)
        {
            return this._Service.AddTaskItem(add.TaskId, add.Dataum);
        }

        public void DeleteTaskItem (DeleteTaskItem item)
        {
            this._Service.Delete(item.ItemId);
        }

        public TaskItemDatum GetTaskItem (GetTaskItem obj)
        {
            return this._Service.Get(obj.Id);
        }

        public TaskItem[] GetsTaskItemByTaskId (GetsTaskItemByTaskId obj)
        {
            return this._Service.GetsByTaskId(obj.TaskId);
        }

        public bool SetTaskItem (SetTaskItem set)
        {
            return this._Service.SetTaskItem(set.ItemId, set.Datum);
        }
    }
}
