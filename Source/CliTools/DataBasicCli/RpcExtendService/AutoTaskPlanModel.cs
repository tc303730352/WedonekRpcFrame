namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskPlan", "自动任务计划")]
    public class AutoTaskPlanModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "任务计划ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "任务ID")]
        public long TaskId { get; set; }

        [SugarColumn(ColumnDescription = "计划名", Length = 50)]
        public string PlanTitle { get; set; }

        [SugarColumn(ColumnDescription = "计划说明", Length = 255, IsNullable = true)]
        public string PlanShow { get; set; }

        [SugarColumn(ColumnDescription = "计划类型")]
        public byte PlanType { get; set; }


        [SugarColumn(ColumnDescription = "执行时间")]
        public DateTime? ExecTime { get; set; }

        [SugarColumn(ColumnDescription = "执行周期")]
        public byte ExecRate { get; set; }


        [SugarColumn(ColumnDescription = "执行间隔")]
        public short? ExecSpace { get; set; }

        [SugarColumn(ColumnDescription = "月间隔类型")]
        public byte SpaceType { get; set; }

        [SugarColumn(ColumnDescription = "间隔天数")]
        public short? SpaceDay { get; set; }

        [SugarColumn(ColumnDescription = "间隔数")]
        public byte? SpeceNum { get; set; }

        [SugarColumn(ColumnDescription = "间隔周期")]
        public byte? SpaceWeek { get; set; }

        [SugarColumn(ColumnDescription = "每天频率")]
        public byte DayRate { get; set; }

        [SugarColumn(ColumnDescription = "间隔的秒数")]
        public int? DayTimeSpan { get; set; }


        [SugarColumn(ColumnDescription = "执行间隔类型")]
        public byte DaySpaceType { get; set; }

        [SugarColumn(ColumnDescription = "每天间隔数")]
        public int? DaySpaceNum { get; set; }

        [SugarColumn(ColumnDescription = "天开始时间（秒）")]
        public int? DayBeginSpan { get; set; }

        [SugarColumn(ColumnDescription = "天结束时间（秒）")]
        public int? DayEndSpan { get; set; }

        [SugarColumn(ColumnDescription = "持续开始时间", ColumnDataType = "date")]
        public DateTime BeginDate { get; set; }

        [SugarColumn(ColumnDescription = "持续截止时间", ColumnDataType = "date")]
        public DateTime? EndDate { get; set; }

        [SugarColumn(ColumnDescription = "计划的唯一键", Length = 32, ColumnDataType = "varchar", IsNullable = true)]
        public string PlanOnlyNo { get; set; }

        [SugarColumn(ColumnDescription = "是否启用")]
        public bool IsEnable { get; set; }
    }
}
