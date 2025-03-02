namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;
    [SugarIndex("IX_TaskId", "TaskId", OrderByType.Asc, false)]
    [SugarTable("AutoTaskItem", TableDescription = "�Զ��������")]
    public class AutoTaskItemModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "������ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "����ID")]
        public long TaskId { get; set; }

        [SugarColumn(ColumnDescription = "���������", Length = 50)]
        public string ItemTitle { get; set; }

        [SugarColumn(ColumnDescription = "����˳��")]
        public int ItemSort { get; set; }

        [SugarColumn(ColumnDescription = "���ͷ�ʽ")]
        public byte SendType { get; set; }

        [SugarColumn(ColumnDescription = "���Ͳ���")]
        public string SendParam { get; set; }

        [SugarColumn(ColumnDescription = "��ʱʱ��(��)")]
        public int? TimeOut { get; set; }

        [SugarColumn(ColumnDescription = "��������Դ���")]
        public short? RetryNum { get; set; }


        [SugarColumn(ColumnDescription = "ʧ��ʱִ�в���")]
        public byte FailStep { get; set; }

        [SugarColumn(ColumnDescription = "ʧ��ʱִ�еĲ����")]
        public int? FailNextStep { get; set; }

        [SugarColumn(ColumnDescription = "�ɹ�ʱִ�еĲ���")]
        public byte SuccessStep { get; set; }

        [SugarColumn(ColumnDescription = "�ɹ�ʱִ�еĲ����")]
        public int? NextStep { get; set; }

        [SugarColumn(ColumnDescription = "��־��¼��Χ")]
        public byte LogRange { get; set; }

        [SugarColumn(ColumnDescription = "�����Ƿ�ִ�гɹ�")]
        public bool IsSuccess { get; set; }

        [SugarColumn(ColumnDescription = "���һ��ִ�д�����")]
        public string Error { get; set; }

        [SugarColumn(ColumnDescription = "���һ��ִ��ʱ��")]
        public DateTime? LastExecTime { get; set; }
    }
}
