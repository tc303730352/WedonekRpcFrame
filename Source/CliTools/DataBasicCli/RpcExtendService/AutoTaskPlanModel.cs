namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AutoTaskPlan", "�Զ�����ƻ�")]
    public class AutoTaskPlanModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "����ƻ�ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "����ID")]
        public long TaskId { get; set; }

        [SugarColumn(ColumnDescription = "�ƻ���", Length = 50)]
        public string PlanTitle { get; set; }

        [SugarColumn(ColumnDescription = "�ƻ�˵��", Length = 255, IsNullable = true)]
        public string PlanShow { get; set; }

        [SugarColumn(ColumnDescription = "�ƻ�����")]
        public byte PlanType { get; set; }


        [SugarColumn(ColumnDescription = "ִ��ʱ��")]
        public DateTime? ExecTime { get; set; }

        [SugarColumn(ColumnDescription = "ִ������")]
        public byte ExecRate { get; set; }


        [SugarColumn(ColumnDescription = "ִ�м��")]
        public short? ExecSpace { get; set; }

        [SugarColumn(ColumnDescription = "�¼������")]
        public byte SpaceType { get; set; }

        [SugarColumn(ColumnDescription = "�������")]
        public short? SpaceDay { get; set; }

        [SugarColumn(ColumnDescription = "�����")]
        public byte? SpeceNum { get; set; }

        [SugarColumn(ColumnDescription = "�������")]
        public byte? SpaceWeek { get; set; }

        [SugarColumn(ColumnDescription = "ÿ��Ƶ��")]
        public byte DayRate { get; set; }

        [SugarColumn(ColumnDescription = "���������")]
        public int? DayTimeSpan { get; set; }


        [SugarColumn(ColumnDescription = "ִ�м������")]
        public byte DaySpaceType { get; set; }

        [SugarColumn(ColumnDescription = "ÿ������")]
        public int? DaySpaceNum { get; set; }

        [SugarColumn(ColumnDescription = "�쿪ʼʱ�䣨�룩")]
        public int? DayBeginSpan { get; set; }

        [SugarColumn(ColumnDescription = "�����ʱ�䣨�룩")]
        public int? DayEndSpan { get; set; }

        [SugarColumn(ColumnDescription = "������ʼʱ��", ColumnDataType = "date")]
        public DateTime BeginDate { get; set; }

        [SugarColumn(ColumnDescription = "������ֹʱ��", ColumnDataType = "date")]
        public DateTime? EndDate { get; set; }

        [SugarColumn(ColumnDescription = "�ƻ���Ψһ��", Length = 32, ColumnDataType = "varchar", IsNullable = true)]
        public string PlanOnlyNo { get; set; }

        [SugarColumn(ColumnDescription = "�Ƿ�����")]
        public bool IsEnable { get; set; }
    }
}
