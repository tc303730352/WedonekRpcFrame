namespace AutoTask.Model.DB
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskLog")]
    public class AutoTaskLogModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public long TaskId { get; set; }

        public long ItemId { get; set; }

        public string ItemTitle { get; set; }
        public bool IsFail { get; set; }

        public string Error { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Result { get; set; }
    }
}
