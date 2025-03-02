namespace AutoTask.Model.DB
{
    using System;
    using RpcTaskModel;
    using SqlSugar;

    [SugarTable("AutoTaskPlan")]
    public class AutoTaskPlanModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long TaskId { get; set; }

        public string PlanTitle { get; set; }
        public string PlanShow { get; set; }

        public TaskPlanType PlanType { get; set; }

        public DateTime? ExecTime { get; set; }

        public TaskExecRate ExecRate { get; set; }

        public short? ExecSpace { get; set; }

        public TaskSpaceType SpaceType { get; set; }

        public short? SpaceDay { get; set; }

        public byte? SpeceNum { get; set; }

        public TaskSpaceWeek? SpaceWeek { get; set; }

        public TaskDayRate DayRate { get; set; }

        public int? DayTimeSpan { get; set; }

        public TaskDaySpaceType DaySpaceType { get; set; }

        public int? DaySpaceNum { get; set; }

        public int? DayBeginSpan { get; set; }

        public int? DayEndSpan { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PlanOnlyNo { get; set; }

        public bool IsEnable { get; set; }
    }
}
