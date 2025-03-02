using SqlSugar;

namespace DataBasicCli.RpcExtendService
{
    [SugarTable("IdentityApp", "��ݱ�ʶӦ�ñ�")]
    public class IdentityAppModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "��ʶID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "��ݱ�ʶAppId", ColumnDataType = "varchar", Length = 32)]
        public string AppId { get; set; }

        [SugarColumn(ColumnDescription = "��ݱ�ʶ��", Length = 50)]
        public string AppName { get; set; }
        [SugarColumn(ColumnDescription = "��ע", Length = 255, IsNullable = true)]
        public string AppShow { get; set; }

        /// <summary>
        /// Ӧ����չ
        /// </summary>
        [SugarColumn(ColumnDescription = "��չ����", IsNullable = true)]
        public string AppExtend { get; set; }

        [SugarColumn(ColumnDescription = "��Чʱ��", ColumnDataType = "date")]
        public DateTime? EffectiveDate { get; set; }
        [SugarColumn(ColumnDescription = "�Ƿ�����")]
        public bool IsEnable { get; set; }
        [SugarColumn(ColumnDescription = "����ʱ��")]
        public DateTime CreateTime { get; set; }
    }
}
