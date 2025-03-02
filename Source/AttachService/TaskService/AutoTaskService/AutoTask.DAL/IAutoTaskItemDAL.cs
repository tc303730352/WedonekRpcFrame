using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.TaskItem;
using RpcTaskModel.TaskItem.Model;

namespace AutoTask.DAL
{
    public interface IAutoTaskItemDAL
    {
        long Add (AutoTaskItemModel item);
        AutoTaskItemModel Get (long id);
        void Delete (long id);
        AutoTaskItemData[] Gets (long taskId);
        AutoTaskItem[] GetTaskItems (long taskId);
        void SyncState (SyncItemResult[] datas);

        void Set (long id, TaskSetItem item);
        TaskItemBasic[] GetTaskItemBasic (long taskId);
        void Clear (long taskId);
        bool CheckItemTitle (long taskId, string itemTitle);

        bool CheckItemSort (long taskId, int itemSort);
        Dictionary<long, string> GetsItemName (long[] ids);
        TaskSelectItem[] GetSelectItems (long taskId);
    }
}