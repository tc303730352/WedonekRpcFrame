using RpcTaskModel.TaskItem.Model;

namespace AutoTask.Service
{
    public interface ITaskItemService
    {
        long AddTaskItem (long taskId, TaskItemSetParam param);
        void Delete (long itemId);
        TaskItemDatum Get (long id);
        TaskItem[] GetsByTaskId (long taskId);
        TaskSelectItem[] GetSelectItems (long taskId);
        bool SetTaskItem (long itemId, TaskItemSetParam param);
    }
}