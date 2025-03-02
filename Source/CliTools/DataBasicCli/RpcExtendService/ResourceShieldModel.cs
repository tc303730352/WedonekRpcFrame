namespace DataBasicCli.RpcExtendService
{
    using SqlSugar;
    /// <summary>
    /// �ӿ���Դ����
    /// </summary>
    [SugarTable("ResourceShield", "��Դ���α�")]
    public class ResourceShieldModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "��ԴID")]
        public long ResourceId { get; set; }

        [SugarColumn(ColumnDescription = "����key", ColumnDataType = "varchar", Length = 36)]
        public string ShieIdKey { get; set; }
        [SugarColumn(ColumnDescription = "��ȺID")]
        public long RpcMerId { get; set; }
        [SugarColumn(ColumnDescription = "�������ֵ", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }
        [SugarColumn(ColumnDescription = "����ڵ�ID")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "���ε�Ӧ�ð汾��")]
        public int VerNum { get; set; }
        [SugarColumn(ColumnDescription = "�����")]
        public short SortNum { get; set; }

        [SugarColumn(ColumnDescription = "��������")]
        public byte ShieldType { get; set; }

        [SugarColumn(ColumnDescription = "��Դ���·��", ColumnDataType = "varchar", Length = 100)]
        public string ResourcePath { get; set; }
        [SugarColumn(ColumnDescription = "����ʱ��")]
        public long BeOverdueTime { get; set; }
        [SugarColumn(ColumnDescription = "����˵��", Length = 100, IsNullable = true)]
        public string ShieIdShow { get; set; }
    }
}
