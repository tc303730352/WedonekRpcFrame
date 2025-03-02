namespace AutoTask.Model.DB
{
    using System;
    using RpcTaskModel;
    using SqlSugar;

    [SugarTable("AutoTaskList")]
    public class AutoTaskModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public int? RegionId { get; set; }

        public long RpcMerId { get; set; }
        public string TaskName { get; set; }

        public string TaskShow { get; set; }

        public int TaskPriority { get; set; }

        public short BeginStep { get; set; }
        [SugarColumn(IsJson = true)]
        public string[] FailEmall { get; set; }

        public int VerNum { get; set; }

        public bool IsExec { get; set; }

        public int ExecVerNum { get; set; }

        public DateTime? LastExecTime { get; set; }

        public DateTime? LastExecEndTime { get; set; }

        public DateTime? NextExecTime { get; set; }

        public AutoTaskStatus TaskStatus { get; set; }

        public DateTime? StopTime { get; set; }
    }
}
