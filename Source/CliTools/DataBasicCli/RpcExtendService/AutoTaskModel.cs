namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskList", "自动任务表")]
    public class AutoTaskModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "任务ID")]
        public long Id { get; set; }
        [SugarColumn(ColumnDescription = "应用机房ID")]
        public int? RegionId { get; set; }

        [SugarColumn(ColumnDescription = "应用集群ID")]
        public long RpcMerId { get; set; }

        [SugarColumn(ColumnDescription = "任务名", Length = 50)]
        public string TaskName { get; set; }

        [SugarColumn(ColumnDescription = "任务说明", Length = 255)]
        public string TaskShow { get; set; }

        [SugarColumn(ColumnDescription = "任务优先级")]
        public int TaskPriority { get; set; }

        [SugarColumn(ColumnDescription = "开始步骤")]
        public short BeginStep { get; set; }

        [SugarColumn(ColumnDescription = "失败时发送Email", Length = 1000, ColumnDataType = "varchar", IsNullable = true)]
        public string FailEmall { get; set; }

        [SugarColumn(ColumnDescription = "版本号")]
        public int VerNum { get; set; }

        [SugarColumn(ColumnDescription = "是否正在执行中")]
        public bool IsExec { get; set; }

        [SugarColumn(ColumnDescription = "执行版本号")]
        public int ExecVerNum { get; set; }

        [SugarColumn(ColumnDescription = "最后执行时间")]
        public DateTime? LastExecTime { get; set; }

        [SugarColumn(ColumnDescription = "最后执行结束时间")]
        public DateTime? LastExecEndTime { get; set; }

        [SugarColumn(ColumnDescription = "下次执行时间")]
        public DateTime? NextExecTime { get; set; }

        [SugarColumn(ColumnDescription = "任务状态")]
        public byte TaskStatus { get; set; }

        [SugarColumn(ColumnDescription = "停止执行时间")]
        public DateTime? StopTime { get; set; }
    }
}
