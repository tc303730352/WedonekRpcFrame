namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;
    [SugarIndex("IX_TaskId", "TaskId", OrderByType.Asc, false)]
    [SugarTable("AutoTaskItem", TableDescription = "自动任务步骤表")]
    public class AutoTaskItemModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "任务项ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "任务ID")]
        public long TaskId { get; set; }

        [SugarColumn(ColumnDescription = "任务项标题", Length = 50)]
        public string ItemTitle { get; set; }

        [SugarColumn(ColumnDescription = "任务顺序")]
        public int ItemSort { get; set; }

        [SugarColumn(ColumnDescription = "发送方式")]
        public byte SendType { get; set; }

        [SugarColumn(ColumnDescription = "发送参数")]
        public string SendParam { get; set; }

        [SugarColumn(ColumnDescription = "超时时间(秒)")]
        public int? TimeOut { get; set; }

        [SugarColumn(ColumnDescription = "允许的重试次数")]
        public short? RetryNum { get; set; }


        [SugarColumn(ColumnDescription = "失败时执行步骤")]
        public byte FailStep { get; set; }

        [SugarColumn(ColumnDescription = "失败时执行的步骤号")]
        public int? FailNextStep { get; set; }

        [SugarColumn(ColumnDescription = "成功时执行的步骤")]
        public byte SuccessStep { get; set; }

        [SugarColumn(ColumnDescription = "成功时执行的步骤号")]
        public int? NextStep { get; set; }

        [SugarColumn(ColumnDescription = "日志记录范围")]
        public byte LogRange { get; set; }

        [SugarColumn(ColumnDescription = "最后次是否执行成功")]
        public bool IsSuccess { get; set; }

        [SugarColumn(ColumnDescription = "最后一次执行错误码")]
        public string Error { get; set; }

        [SugarColumn(ColumnDescription = "最后一次执行时间")]
        public DateTime? LastExecTime { get; set; }
    }
}
