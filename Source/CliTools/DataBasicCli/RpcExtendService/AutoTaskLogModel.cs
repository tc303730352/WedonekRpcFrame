namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskLog", "�Զ�������־��")]
    public class AutoTaskLogModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "��־ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "����ID")]
        public long TaskId { get; set; }

        [SugarColumn(ColumnDescription = "ִ�е�������ID")]
        public long ItemId { get; set; }

        [SugarColumn(ColumnDescription = "ִ�е����������", Length = 50)]
        public string ItemTitle { get; set; }

        [SugarColumn(ColumnDescription = "�Ƿ�ʧ��")]
        public bool IsFail { get; set; }

        [SugarColumn(ColumnDescription = "������")]
        public string Error { get; set; }

        [SugarColumn(ColumnDescription = "ִ�п�ʼʱ��")]
        public DateTime BeginTime { get; set; }

        [SugarColumn(ColumnDescription = "ִ�н���ʱ��")]
        public DateTime EndTime { get; set; }

        [SugarColumn(ColumnDescription = "ִ�н��", Length = 1000, IsNullable = true)]
        public string Result { get; set; }
    }
}
