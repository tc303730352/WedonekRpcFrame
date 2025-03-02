using AutoTask.DAL;
using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.TaskItem;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;

namespace AutoTask.Collect.Collect
{
    internal class TaskItemCollect : ITaskItemCollect
    {
        private readonly IAutoTaskItemDAL _BasicDAL;
        private readonly ICacheController _Cache;
        public TaskItemCollect (IAutoTaskItemDAL dal,
            ICacheController cache)
        {
            this._Cache = cache;
            this._BasicDAL = dal;
        }

        public long Add (long taskId, TaskItemSetParam item)
        {
            if (this._BasicDAL.CheckItemTitle(taskId, item.ItemTitle))
            {
                throw new ErrorException("task.item.title.repeat");
            }
            AutoTaskItemModel add = item.ConvertMap<TaskItemSetParam, AutoTaskItemModel>();
            add.TaskId = taskId;
            add.Id = this._BasicDAL.Add(add);
            return add.Id;
        }

        public TaskItemBasic[] GetTaskItemBasic (long taskId)
        {
            return this._BasicDAL.GetTaskItemBasic(taskId);
        }


        public void Delete (AutoTaskItemModel item)
        {
            this._BasicDAL.Delete(item.Id);
        }
        public Dictionary<long, string> GetsItemName (long[] ids)
        {
            return this._BasicDAL.GetsItemName(ids);
        }
        public AutoTaskItemModel Get (long id)
        {
            AutoTaskItemModel item = this._BasicDAL.Get(id);
            if (item == null)
            {
                throw new ErrorException("task.item.not.find");
            }
            return item;
        }
        public AutoTaskItem[] Gets (long taskId, int verNum)
        {
            string key = string.Join("_", "TaskItem", taskId, verNum);
            if (this._Cache.TryGet(key, out AutoTaskItem[] items))
            {
                return items;
            }
            items = this._BasicDAL.GetTaskItems(taskId);
            if (items == null)
            {
                items = new AutoTaskItem[0];
            }
            _ = this._Cache.Set(key, items, new TimeSpan(1, 0, 0, 0));
            return items;
        }

        public AutoTaskItemData[] Gets (long taskId)
        {
            return this._BasicDAL.Gets(taskId);
        }

        public bool Set (AutoTaskItemModel item, TaskItemSetParam set)
        {
            if (set.IsEquals(item))
            {
                return false;
            }
            else if (item.ItemTitle != set.ItemTitle && this._BasicDAL.CheckItemTitle(item.TaskId, set.ItemTitle))
            {
                throw new ErrorException("task.item.title.repeat");
            }
            TaskSetItem setItem = set.ConvertMap<TaskItemSetParam, TaskSetItem>();
            this._BasicDAL.Set(item.Id, setItem);
            return true;
        }


        public void SyncState (SyncItemResult[] results)
        {
            this._BasicDAL.SyncState(results);
        }
        public TaskSelectItem[] GetSelectItems (long taskId)
        {
            return this._BasicDAL.GetSelectItems(taskId);
        }
        public void CheckItemTitle (long taskId, string itemTitle)
        {
            if (this._BasicDAL.CheckItemTitle(taskId, itemTitle))
            {
                throw new ErrorException("task.item.title.repeat");
            }
        }
    }
}
