namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskList", "�Զ������")]
    public class AutoTaskModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "����ID")]
        public long Id { get; set; }
        [SugarColumn(ColumnDescription = "Ӧ�û���ID")]
        public int? RegionId { get; set; }

        [SugarColumn(ColumnDescription = "Ӧ�ü�ȺID")]
        public long RpcMerId { get; set; }

        [SugarColumn(ColumnDescription = "������", Length = 50)]
        public string TaskName { get; set; }

        [SugarColumn(ColumnDescription = "����˵��", Length = 255)]
        public string TaskShow { get; set; }

        [SugarColumn(ColumnDescription = "�������ȼ�")]
        public int TaskPriority { get; set; }

        [SugarColumn(ColumnDescription = "��ʼ����")]
        public short BeginStep { get; set; }

        [SugarColumn(ColumnDescription = "ʧ��ʱ����Email", Length = 1000, ColumnDataType = "varchar", IsNullable = true)]
        public string FailEmall { get; set; }

        [SugarColumn(ColumnDescription = "�汾��")]
        public int VerNum { get; set; }

        [SugarColumn(ColumnDescription = "�Ƿ�����ִ����")]
        public bool IsExec { get; set; }

        [SugarColumn(ColumnDescription = "ִ�а汾��")]
        public int ExecVerNum { get; set; }

        [SugarColumn(ColumnDescription = "���ִ��ʱ��")]
        public DateTime? LastExecTime { get; set; }

        [SugarColumn(ColumnDescription = "���ִ�н���ʱ��")]
        public DateTime? LastExecEndTime { get; set; }

        [SugarColumn(ColumnDescription = "�´�ִ��ʱ��")]
        public DateTime? NextExecTime { get; set; }

        [SugarColumn(ColumnDescription = "����״̬")]
        public byte TaskStatus { get; set; }

        [SugarColumn(ColumnDescription = "ִֹͣ��ʱ��")]
        public DateTime? StopTime { get; set; }
    }
}
