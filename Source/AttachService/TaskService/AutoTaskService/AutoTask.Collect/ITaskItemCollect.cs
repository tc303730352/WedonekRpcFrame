using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.TaskItem;
using RpcTaskModel.TaskItem.Model;

namespace AutoTask.Collect
{
    public interface ITaskItemCollect
    {
        Dictionary<long, string> GetsItemName (long[] ids);
        bool Set (AutoTaskItemModel item, TaskItemSetParam set);
        AutoTaskItemData[] Gets (long taskId);
        AutoTaskItemModel Get (long id);
        void Delete (AutoTaskItemModel item);
        long Add (long id, TaskItemSetParam item);
        void SyncState (SyncItemResult[] results);
        AutoTaskItem[] Gets (long taskId, int verNum);
        TaskItemBasic[] GetTaskItemBasic (long taskId);
        void CheckItemTitle (long taskId, string itemTitle);
        TaskSelectItem[] GetSelectItems (long taskId);
    }
}