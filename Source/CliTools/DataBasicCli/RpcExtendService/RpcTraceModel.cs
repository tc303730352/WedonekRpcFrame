namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("RpcTraceList", "����·��־��")]
    public class RpcTraceModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "��־ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "��·TraceId", Length = 32, ColumnDataType = "varchar")]
        public string TraceId { get; set; }

        [SugarColumn(ColumnDescription = "ִ�е�ָ���Uri", ColumnDataType = "varchar", Length = 1000)]
        public string Dictate { get; set; }

        [SugarColumn(ColumnDescription = "��ע˵��", Length = 50, IsNullable = true)]
        public string Show { get; set; }


        [SugarColumn(ColumnDescription = "��¼����ʱ��")]
        public DateTime BeginTime { get; set; }

        [SugarColumn(ColumnDescription = "��ʱ")]
        public int Duration { get; set; }

        [SugarColumn(ColumnDescription = "����ķ���ڵ�")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "�������ֵ", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }


        [SugarColumn(ColumnDescription = "����ID")]
        public int RegionId { get; set; }

        [SugarColumn(ColumnDescription = "��ȺID")]
        public long RpcMerId { get; set; }
    }
}
