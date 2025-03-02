namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("RpcTraceLog", "��·��־��")]
    public class RpcTraceLogModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "��־ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "��·ID", Length = 32, ColumnDataType = "varchar")]
        public string TraceId { get; set; }

        [SugarColumn(ColumnDescription = "SpanId")]
        public long SpanId { get; set; }

        [SugarColumn(ColumnDescription = "����SpanId")]
        public long ParentId { get; set; }

        [SugarColumn(ColumnDescription = "ִ�е�ָ���Uri", ColumnDataType = "varchar", Length = 1000)]
        public string Dictate { get; set; }

        [SugarColumn(ColumnDescription = "��ע˵��", Length = 50)]
        public string Show { get; set; }

        [SugarColumn(ColumnDescription = "�������ڵ�Id")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "�������ֵ", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }


        [SugarColumn(ColumnDescription = "Ŀ�ĵط���ڵ�Id")]
        public long RemoteId { get; set; }

        [SugarColumn(ColumnDescription = "����ID")]
        public int RegionId { get; set; }


        [SugarColumn(ColumnDescription = "ִ�н��")]
        public string ReturnRes { get; set; }

        [SugarColumn(ColumnDescription = "���Ͳ���")]
        public string Args { get; set; }

        [SugarColumn(ColumnDescription = "��Ϣ����", Length = 10, ColumnDataType = "varchar", IsNullable = true)]
        public string MsgType { get; set; }

        [SugarColumn(ColumnDescription = "��·����")]
        public byte StageType { get; set; }

        [SugarColumn(ColumnDescription = "��ʼ��¼ʱ��")]
        public DateTime BeginTime { get; set; }

        [SugarColumn(ColumnDescription = "��ʱ")]
        public int Duration { get; set; }
    }
}
