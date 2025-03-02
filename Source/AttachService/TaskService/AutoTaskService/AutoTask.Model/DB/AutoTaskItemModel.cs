namespace AutoTask.Model.DB
{
    using System;
    using RpcTaskModel;
    using RpcTaskModel.TaskItem.Model;
    using SqlSugar;

    [SugarTable("AutoTaskItem")]
    public class AutoTaskItemModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long TaskId { get; set; }

        public string ItemTitle { get; set; }

        public int ItemSort { get; set; }

        public TaskSendType SendType { get; set; }
        [SugarColumn(IsJson = true, ColumnDataType = "nvarchar(max)")]
        public SendParam SendParam { get; set; }

        public int? TimeOut { get; set; }

        public short? RetryNum { get; set; }

        public TaskStep FailStep { get; set; }

        public int? FailNextStep { get; set; }

        public TaskStep SuccessStep { get; set; }

        public int? NextStep { get; set; }

        public TaskLogRange LogRange { get; set; }

        public bool IsSuccess { get; set; }

        public string Error { get; set; }

        public DateTime? LastExecTime { get; set; }
    }
}
