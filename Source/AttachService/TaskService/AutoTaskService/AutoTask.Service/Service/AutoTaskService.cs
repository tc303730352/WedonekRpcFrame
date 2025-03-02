using AutoTask.Collect;
using AutoTask.Model.DB;
using AutoTask.Model.Task;
using AutoTask.Model.TaskItem;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcTaskModel;
using RpcTaskModel.AutoTask.Model;
using RpcTaskModel.AutoTask.Msg;

namespace AutoTask.Service.Service
{
    internal class AutoTaskService : IAutoTaskService
    {
        private readonly IAutoTaskCollect _AutoTask;
        private readonly ITaskPlanCollect _TaskPlan;
        private readonly ITaskItemCollect _TaskItem;
        public AutoTaskService (IAutoTaskCollect task,
            ITaskPlanCollect taskPlan,
            ITaskItemCollect taskItem)
        {
            this._AutoTask = task;
            this._TaskPlan = taskPlan;
            this._TaskItem = taskItem;
        }
        public long Add (AutoTaskAdd task)
        {
            return this._AutoTask.Add(task);
        }
        public PagingResult<AutoTaskBasic> Query (TaskQueryParam query, IBasicPage paging)
        {
            AutoTaskDatum[] list = this._AutoTask.Query(query, paging, out int count);
            return new PagingResultTo<AutoTaskDatum, AutoTaskBasic>(count, list);
        }
        public AutoTaskInfo Get (long taskId)
        {
            AutoTaskModel task = this._AutoTask.Get(taskId);
            return task.ConvertMap<AutoTaskModel, AutoTaskInfo>();
        }
        public bool DisableTask (long taskId, bool isEndTask)
        {
            AutoTaskModel task = this._AutoTask.Get(taskId);
            if (task.TaskStatus != AutoTaskStatus.启用)
            {
                throw new ErrorException("task.no.enable");
            }
            else if (this._AutoTask.SetTaskStatus(task, AutoTaskStatus.停用))
            {
                EndTaskEvent send = new EndTaskEvent
                {
                    TaskId = taskId,
                    IsEnd = isEndTask
                };
                if (task.RegionId.HasValue)
                {
                    send.SyncSend(task.RegionId.Value, task.RpcMerId);
                }
                else
                {
                    send.SyncSend(task.RpcMerId);
                }
                return true;
            }
            return false;
        }
        public bool EnableTask (long taskId)
        {
            AutoTaskModel task = this._AutoTask.Get(taskId);
            if (task.TaskStatus == AutoTaskStatus.启用)
            {
                return false;
            }
            this._CheckTask(task);
            return this._AutoTask.SetTaskStatus(task, AutoTaskStatus.启用);
        }
        private void _CheckTask (AutoTaskModel task)
        {
            if (task.BeginStep <= 0)
            {
                throw new ErrorException("task.begin.item.null");
            }
            this._TaskPlan.CheckTaskPlan(task.Id);
            TaskItemBasic[] items = this._TaskItem.GetTaskItemBasic(task.Id);
            if (items.IsNull())
            {
                throw new ErrorException("task.item.no.enable");
            }
            else if (!items.IsExists(c => c.SuccessStep == TaskStep.停止执行))
            {
                //没有停止节点
                throw new ErrorException("task.item.step.no.end");
            }
            else if (!items.IsExists(c => c.ItemSort == task.BeginStep))
            {
                //未找到开始节点
                throw new ErrorException("task.begin.item.not.find");
            }
            //按照序号排序
            items = items.OrderBy(a => a.ItemSort).ToArray();
            items.ForEach(a =>
            {
                if (a.FailStep == TaskStep.执行指定步骤 && !items.IsExists(c => c.ItemSort == a.FailNextStep.Value))
                {
                    throw new ErrorException(string.Format("task.item.fail.step.not.find[id={0},title={1}]", a.Id, a.ItemTitle));
                }
                else if (a.SuccessStep == TaskStep.停止执行 && a.FailStep == TaskStep.停止执行)
                {
                    return;
                }
                else if (a.SuccessStep == TaskStep.执行指定步骤 && !items.IsExists(c => c.ItemSort == a.NextStep.Value))
                {
                    throw new ErrorException(string.Format("task.item.next.step.not.find[id={0},title={1}]", a.Id, a.ItemTitle));
                }
            });
            if (!items.TrueForAll(a =>
            {
                return this._CheckIsEnd(a, items);
            }))
            {
                throw new ErrorException("task.item.step.cannot.end");
            }
        }
        private bool _CheckIsEnd (TaskItemBasic item, TaskItemBasic[] items)
        {
            if (item.SuccessStep == TaskStep.停止执行 && item.FailStep == TaskStep.停止执行)
            {
                return true;
            }
            int sort = item.SuccessStep == TaskStep.执行指定步骤 ? item.NextStep.Value : item.ItemSort + 1;
            if (sort <= item.ItemSort)
            {
                //存在死循环
                throw new ErrorException(string.Format("task.item.next.step.repeat[id={0},title={1}]", item.Id, item.ItemTitle));
            }
            TaskItemBasic[] subs = items.FindAll(c => c.ItemSort == sort);
            if (subs.IsNull())
            {
                return false;
            }
            return subs.TrueForAll(c =>
            {
                return this._CheckIsEnd(c, items);
            });
        }
        public bool Set (long taskId, AutoTaskSet datum)
        {
            AutoTaskModel task = this._AutoTask.Get(taskId);
            if (task.TaskStatus == AutoTaskStatus.启用)
            {
                throw new ErrorException("task.already.enable");
            }
            return this._AutoTask.Set(task, datum);
        }
        public void Delete (long taskId)
        {
            AutoTaskModel task = this._AutoTask.Get(taskId);
            if (task.TaskStatus == AutoTaskStatus.启用)
            {
                throw new ErrorException("task.already.enable");
            }
            this._AutoTask.Delete(task);
        }
    }
}
