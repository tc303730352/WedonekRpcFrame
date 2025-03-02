using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Validate;
using RpcTaskModel;

namespace AutoTask.Model.TaskPlan
{
    public class AutoTaskPlan
    {
        /// <summary>
        /// 计划Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 计划标题
        /// </summary>
        public string PlanTitle { get; set; }
        /// <summary>
        /// 计划类型
        /// </summary>
        public TaskPlanType PlanType { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? ExecTime { get; set; }
        /// <summary>
        /// 执行周期
        /// </summary>
        public TaskExecRate ExecRate { get; set; }
        /// <summary>
        /// 执行间隔
        /// </summary>
        public short? ExecSpace { get; set; }
        /// <summary>
        /// 月间隔类型
        /// </summary>
        public TaskSpaceType SpaceType { get; set; }
        /// <summary>
        /// 间隔天数
        /// </summary>
        public short? SpaceDay { get; set; }
        /// <summary>
        /// 间隔数
        /// </summary>
        public byte? SpeceNum { get; set; }
        /// <summary>
        /// 间隔周数
        /// </summary>
        public TaskSpaceWeek? SpaceWeek { get; set; }
        /// <summary>
        /// 每天频率
        /// </summary>
        public TaskDayRate DayRate { get; set; }
        /// <summary>
        /// 间隔的秒数
        /// </summary>
        public int? DayTimeSpan { get; set; }
        /// <summary>
        /// 执行间隔类型
        /// </summary>
        public TaskDaySpaceType DaySpaceType { get; set; }
        /// <summary>
        /// 每天间隔数
        /// </summary>
        public int? DaySpaceNum { get; set; }
        /// <summary>
        /// 天开始时间（秒）
        /// </summary>
        public int? DayBeginSpan { get; set; }
        /// <summary>
        /// 天结束时间（秒）
        /// </summary>
        public int? DayEndSpan { get; set; }
        /// <summary>
        /// 持续开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }
        /// <summary>
        /// 持续截止时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

    }
}
