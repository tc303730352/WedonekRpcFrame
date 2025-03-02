using AutoTask.Model.DB;
using AutoTask.Model.TaskPlan;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using RpcTaskModel.TaskPlan.Model;
using WeDonekRpc.SqlSugar;

namespace AutoTask.DAL.Repository
{
    internal class AutoTaskPlanDAL : IAutoTaskPlanDAL
    {
        private readonly IRepository<AutoTaskPlanModel> _BasicDAL;
        public AutoTaskPlanDAL (IRepository<AutoTaskPlanModel> dal)
        {
            this._BasicDAL = dal;
        }
        public int GetEnableCount (long taskId)
        {
            return this._BasicDAL.Count(c => c.TaskId == taskId && c.IsEnable);
        }
        public long Add (AutoTaskPlanModel plan)
        {
            plan.Id = IdentityHelper.CreateId();
            this._BasicDAL.Insert(plan);
            return plan.Id;
        }
        public AutoTaskPlanModel[] Gets (long taskId)
        {
            return this._BasicDAL.Gets(c => c.TaskId == taskId);
        }
        public AutoTaskPlanModel Get (long id)
        {
            return this._BasicDAL.Get(c => c.Id == id);
        }
        public void Set (long id, TaskPlanSetParam set)
        {
            if (!this._BasicDAL.Update(set, a => a.Id == id))
            {
                throw new ErrorException("task.plan.set.fail");
            }
        }
        public void SetIsEnable (long id, bool isEnable)
        {
            if (!this._BasicDAL.Update(a => a.IsEnable == isEnable, a => a.Id == id))
            {
                throw new ErrorException("task.plan.set.fail");
            }
        }
        public bool CheckIsRepeat (long taskId, string planOnlyNo)
        {
            return this._BasicDAL.IsExist(c => c.TaskId == taskId && c.PlanOnlyNo == planOnlyNo);
        }
        public void Delete (long id)
        {
            if (!this._BasicDAL.Delete(a => a.Id == id))
            {
                throw new ErrorException("task.plan.delete.fail");
            }
        }
        public void Clear (long taskId)
        {
            _ = this._BasicDAL.Delete(c => c.TaskId == taskId);
        }
        public TaskPlanBasic[] GetTaskPlans (long taskId)
        {
            return this._BasicDAL.Gets(a => a.TaskId == taskId && a.IsEnable, c => new TaskPlanBasic
            {
                BeginDate = c.BeginDate,
                SpaceDay = c.SpaceDay,
                SpaceType = c.SpaceType,
                SpaceWeek = c.SpaceWeek,
                SpeceNum = c.SpeceNum,
                DayBeginSpan = c.DayBeginSpan,
                DayEndSpan = c.DayEndSpan,
                DayRate = c.DayRate,
                DaySpaceNum = c.DaySpaceNum,
                DaySpaceType = c.DaySpaceType,
                DayTimeSpan = c.DayTimeSpan,
                EndDate = c.EndDate,
                ExecRate = c.ExecRate,
                ExecSpace = c.ExecSpace,
                ExecTime = c.ExecTime,
                Id = c.Id,
                IsEnable = c.IsEnable,
                PlanTitle = c.PlanTitle,
                PlanType = c.PlanType
            });
        }

    }
}
