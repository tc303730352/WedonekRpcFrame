using AutoTask.Collect;
using AutoTask.Model.DB;
using AutoTask.Model.TaskItem;
using RpcTaskModel;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

namespace AutoTask.Service.Service
{
    internal class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemCollect _TaskItem;
        private readonly IAutoTaskCollect _Task;
        public TaskItemService (ITaskItemCollect taskItem, IAutoTaskCollect task)
        {
            this._TaskItem = taskItem;
            this._Task = task;
        }
        public TaskItemDatum Get (long id)
        {
            AutoTaskItemModel item = this._TaskItem.Get(id);
            return item.ConvertMap<AutoTaskItemModel, TaskItemDatum>();
        }
        public void Delete (long itemId)
        {
            AutoTaskItemModel item = this._TaskItem.Get(itemId);
            this._TaskItem.Delete(item);
        }
        public long AddTaskItem (long taskId, TaskItemSetParam param)
        {
            if (this._Task.CheckTaskStatus(taskId, AutoTaskStatus.启用))
            {
                throw new ErrorException("task.already.enable");
            }
            this._TaskItem.CheckItemTitle(taskId, param.ItemTitle);
            return this._TaskItem.Add(taskId, param);
        }
        public bool SetTaskItem (long itemId, TaskItemSetParam param)
        {
            AutoTaskItemModel item = this._TaskItem.Get(itemId);
            if (param.ItemTitle != item.ItemTitle)
            {
                this._TaskItem.CheckItemTitle(itemId, param.ItemTitle);
            }
            if (this._Task.CheckTaskStatus(item.TaskId, AutoTaskStatus.启用))
            {
                throw new ErrorException("task.already.enable");
            }
            return this._TaskItem.Set(item, param);
        }

        public TaskItem[] GetsByTaskId (long taskId)
        {
            AutoTaskItemData[] list = this._TaskItem.Gets(taskId);
            return list.ConvertMap<AutoTaskItemData, TaskItem>();
        }

        public TaskSelectItem[] GetSelectItems (long taskId)
        {
            return this._TaskItem.GetSelectItems(taskId);
        }
    }
}
