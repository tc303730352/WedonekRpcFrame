namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("ResourceModular", "��Դģ��")]
    public class ResourceModularModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "ģ��ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "ģ��KEY", Length = 32, ColumnDataType = "varchar")]
        public string ModularKey { get; set; }
        [SugarColumn(ColumnDescription = "��ȺId")]
        public long RpcMerId { get; set; }
        [SugarColumn(ColumnDescription = "�������ֵ", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }
        [SugarColumn(ColumnDescription = "ģ����", Length = 50)]
        public string ModularName { get; set; }
        [SugarColumn(ColumnDescription = "��ע", Length = 50, IsNullable = true)]
        public string Remark { get; set; }
        [SugarColumn(ColumnDescription = "��Դ����")]
        public byte ResourceType { get; set; }
        [SugarColumn(ColumnDescription = "���ʱ��")]
        public DateTime AddTime { get; set; }
    }
}
