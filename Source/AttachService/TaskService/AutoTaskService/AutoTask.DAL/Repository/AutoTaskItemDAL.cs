using AutoTask.Model;
using AutoTask.Model.DB;
using AutoTask.Model.TaskItem;
using RpcTaskModel.TaskItem.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace AutoTask.DAL.Repository
{
    internal class AutoTaskItemDAL : IAutoTaskItemDAL
    {
        private readonly IRepository<AutoTaskItemModel> _BasicDAL;
        public AutoTaskItemDAL (IRepository<AutoTaskItemModel> dal)
        {
            this._BasicDAL = dal;
        }
        public TaskItemBasic[] GetTaskItemBasic (long taskId)
        {
            return this._BasicDAL.Gets<TaskItemBasic>(c => c.TaskId == taskId);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public long Add (AutoTaskItemModel item)
        {
            item.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(item);
            return item.Id;
        }
        public AutoTaskItemModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public AutoTaskItemData[] Gets (long taskId)
        {
            return this._BasicDAL.Gets<AutoTaskItemData>(c => c.TaskId == taskId, "ItemSort");
        }
        public void Set (long id, TaskSetItem item)
        {
            if (!this._BasicDAL.Update(item, a => a.Id == id))
            {
                throw new ErrorException("task.item.set.fail");
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("task.item.delete.fail");
            }
        }
        public AutoTaskItem[] GetTaskItems (long taskId)
        {
            return this._BasicDAL.Gets<AutoTaskItem>(c => c.TaskId == taskId);
        }
        public TaskSelectItem[] GetSelectItems (long taskId)
        {
            return this._BasicDAL.Gets<TaskSelectItem>(a => a.TaskId == taskId);
        }
        public void SyncState (SyncItemResult[] datas)
        {
            DateTime now = DateTime.Now;
            AutoTaskItemModel[] models = datas.ConvertAll(c => new AutoTaskItemModel
            {
                Id = c.ItemId,
                Error = c.Error,
                IsSuccess = c.IsSuccess,
                LastExecTime = now
            });
            if (!this._BasicDAL.UpdateOnly(models, c => new
            {
                c.IsSuccess,
                c.Error,
                c.LastExecTime
            }))
            {
                throw new ErrorException("task.item.sync.fail");
            }
        }

        public void Clear (long taskId)
        {
            _ = this._BasicDAL.Delete(c => c.TaskId == taskId);
        }
        public Dictionary<long, string> GetsItemName (long[] ids)
        {
            var items = this._BasicDAL.Gets(c => ids.Contains(c.Id), a => new
            {
                a.Id,
                a.ItemTitle
            });
            return items.ToDictionary(a => a.Id, a => a.ItemTitle);
        }
        public bool CheckItemSort (long taskId, int itemSort)
        {
            return this._BasicDAL.IsExist(c => c.TaskId == taskId && c.ItemSort == itemSort);
        }
        public bool CheckItemTitle (long taskId, string itemTitle)
        {
            return this._BasicDAL.IsExist(c => c.TaskId == taskId && c.ItemTitle == itemTitle);
        }
    }
}
