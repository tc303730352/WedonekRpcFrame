namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("ResourceList", "��Դ��")]
    public class ResourceListModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "��ԴID")]
        public long Id { get; set; }
        [SugarColumn(ColumnDescription = "ģ��ID")]
        public long ModularId { get; set; }

        [SugarColumn(ColumnDescription = "��Դ���·��", ColumnDataType = "varchar", Length = 100)]
        public string ResourcePath { get; set; }

        [SugarColumn(ColumnDescription = "��Դ����·��", ColumnDataType = "varchar", Length = 300)]
        public string FullPath { get; set; }

        [SugarColumn(ColumnDescription = "��Դ˵��", Length = 100)]
        public string ResourceShow { get; set; }

        [SugarColumn(ColumnDescription = "��Դ״̬")]
        public byte ResourceState { get; set; }

        [SugarColumn(ColumnDescription = "Ӧ�ð汾��")]
        public int VerNum { get; set; }

        [SugarColumn(ColumnDescription = "��Դ�汾��")]
        public int ResourceVer { get; set; }

        [SugarColumn(ColumnDescription = "������ʱ��", ColumnDataType = "date")]
        public DateTime? LastTime { get; set; }

        [SugarColumn(ColumnDescription = "���ʱ��")]
        public DateTime AddTime { get; set; }
    }
}
