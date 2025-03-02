using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using System;

namespace RpcTaskModel.TaskPlan.Model
{
    public class TaskPlanSet
    {
        /// <summary>
        /// 计划标题
        /// </summary>
        [NullValidate("task.plan.title.null")]
        [LenValidate("task.plan.title.len", 2, 50)]
        public string PlanTitle { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        [EnumValidate("task.plan.type.error", typeof(TaskPlanType))]
        public TaskPlanType PlanType { get; set; }
        /// <summary>
        /// 执行时间
        /// </summary>
        [EntrustValidate("_CheckPlan")]
        [TimeValidate("task.exec.time.error", TimeFormat.秒, 0)]
        public DateTime? ExecTime { get; set; }

        /// <summary>
        /// 执行周期
        /// </summary>
        [EnumValidate("task.plan.exec.rate.error", typeof(TaskExecRate))]
        public TaskExecRate ExecRate { get; set; }

        /// <summary>
        /// 执行间隔
        /// </summary>
        [NumValidate("task.plan.exec.space.error", 1, short.MaxValue)]
        public short? ExecSpace { get; set; }

        /// <summary>
        /// 月间隔类型
        /// </summary>
        [EnumValidate("task.plan.space.type.error", typeof(TaskSpaceType))]
        public TaskSpaceType SpaceType { get; set; }

        /// <summary>
        /// 间隔天数
        /// </summary>
        [NumValidate("task.plan.space.day.error", 1, short.MaxValue)]
        public short? SpaceDay { get; set; }

        /// <summary>
        /// 间隔数
        /// </summary>
        [NumValidate("task.plan.space.num.error", 1, byte.MaxValue)]
        public byte? SpeceNum { get; set; }
        /// <summary>
        /// 间隔周期
        /// </summary>
        [EnumValidate("task.plan.space.type.error", typeof(TaskSpaceWeek), IsContain = true)]
        public TaskSpaceWeek? SpaceWeek { get; set; }
        /// <summary>
        /// 每天频率
        /// </summary>
        [EnumValidate("task.plan.day.rate.error", typeof(TaskDayRate))]
        public TaskDayRate DayRate { get; set; }
        /// <summary>
        /// 间隔的秒数
        /// </summary>
        [NumValidate("task.plan.day.timespan.error", 0, int.MaxValue)]
        public int? DayTimeSpan { get; set; }
        /// <summary>
        /// 执行间隔类型
        /// </summary>
        [EnumValidate("task.plan.day.space.type.error", typeof(TaskDaySpaceType))]
        public TaskDaySpaceType DaySpaceType { get; set; }
        /// <summary>
        /// 每天间隔数
        /// </summary>
        [NumValidate("task.plan.day.space.num.error", 1, int.MaxValue)]
        public int? DaySpaceNum { get; set; }

        /// <summary>
        /// 天开始时间（秒）
        /// </summary>
        [NumValidate("task.plan.day.begin.error", 0, int.MaxValue)]
        public int? DayBeginSpan { get; set; }
        /// <summary>
        /// 天结束时间（秒）
        /// </summary>
        [NumValidate("task.plan.day.end.span.error", 0, int.MaxValue)]
        public int? DayEndSpan { get; set; }
        /// <summary>
        /// 持续开始时间
        /// </summary>
        [TimeValidate("task.plan.begin.date.error", TimeFormat.日, 0, true)]
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 持续截止时间
        /// </summary>
        [TimeValidate("task.plan.end.date.error", TimeFormat.日, 1, true)]
        public DateTime? EndDate { get; set; }

   
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
        private bool _CheckPlan(out string error)
        {
            if (this.PlanType == TaskPlanType.只执行一次 && !this.ExecTime.HasValue)
            {
                error = "task.plan.exec.time.null";
                return false;
            }
            else if (this.PlanType == TaskPlanType.只执行一次)
            {
                error = null;
                return true;
            }
            else if (this.EndDate.HasValue && this.EndDate.Value <= HeartbeatTimeHelper.CurrentDate)
            {
                error = "task.plan.already.end";
                return false;
            }
            else if (this.ExecRate == TaskExecRate.每周)
            {
                if (!this.SpaceWeek.HasValue)
                {
                    error = "task.plan.space.week.null";
                    return false;
                }
            }
            else if (this.ExecRate == TaskExecRate.每月)
            {
                if (this.SpaceType == TaskSpaceType.天 && !this.SpaceDay.HasValue)
                {
                    error = "task.plan.space.day.null";
                    return false;
                }
                else if (this.SpaceType == TaskSpaceType.周)
                {
                    if (!this.SpeceNum.HasValue)
                    {
                        error = "task.plan.space.num.null";
                        return false;
                    }
                    else if (!this.SpaceWeek.HasValue)
                    {
                        error = "task.plan.space.week.null";
                        return false;
                    }
                }
            }
            if (this.DayRate == TaskDayRate.执行一次 && !this.DayTimeSpan.HasValue)
            {
                error = "task.plan.day.timespan.null";
                return false;
            }
            else if (this.DayRate == TaskDayRate.间隔执行)
            {
                if (!this.DayBeginSpan.HasValue)
                {
                    error = "task.plan.day.begin.null";
                    return false;
                }
                else if (!this.DaySpaceNum.HasValue)
                {
                    error = "task.plan.day.space.num.null";
                    return false;
                }
            }
            error = null;
            return true;
        }
    }
}
