namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskLog", "自动任务日志表")]
    public class AutoTaskLogModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "日志ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "任务ID")]
        public long TaskId { get; set; }

        [SugarColumn(ColumnDescription = "执行的任务项ID")]
        public long ItemId { get; set; }

        [SugarColumn(ColumnDescription = "执行的任务项标题", Length = 50)]
        public string ItemTitle { get; set; }

        [SugarColumn(ColumnDescription = "是否失败")]
        public bool IsFail { get; set; }

        [SugarColumn(ColumnDescription = "错误码")]
        public string Error { get; set; }

        [SugarColumn(ColumnDescription = "执行开始时间")]
        public DateTime BeginTime { get; set; }

        [SugarColumn(ColumnDescription = "执行结束时间")]
        public DateTime EndTime { get; set; }

        [SugarColumn(ColumnDescription = "执行结果", Length = 1000, IsNullable = true)]
        public string Result { get; set; }
    }
}
