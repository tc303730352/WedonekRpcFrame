namespace DataBasicCli.RpcExtendService
{
    using SqlSugar;

    [SugarTable("Idgenerator", "Yitter.IdGeneratorѩ����ʶ�㷨WorkId�����")]
    public class IdgeneratorModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "�����WorkId")]
        public int WorkId { get; set; }

        [SugarColumn(ColumnDescription = "����ڵ�����ID")]
        public long SystemTypeId { get; set; }

        /// <summary>
        /// ����ڵ������MAC��ַ
        /// </summary>
        [SugarColumn(ColumnDescription = "����ڵ������MAC��ַ", Length = 17, ColumnDataType = "varchar", IsNullable = false)]
        public string Mac { get; set; }

        [SugarColumn(ColumnDescription = "������")]
        public int ServerIndex { get; set; }
    }
}
