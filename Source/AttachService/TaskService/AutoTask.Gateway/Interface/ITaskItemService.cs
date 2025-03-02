using AutoTask.Gateway.Model;
using RpcTaskModel.TaskItem.Model;

namespace AutoTask.Gateway.Interface
{
    public interface ITaskItemService
    {
        TaskSelectItem[] GetSelectItems (long taskId);
        AutoTaskItem GetDetailed (long id);
        long Add (long taskId, TaskItemSetParam datum);
        void Delete (long id);
        TaskItemDatum Get (long id);
        TaskItem[] Gets (long taskId);
        bool Set (long id, TaskItemSetParam datum);
    }
}