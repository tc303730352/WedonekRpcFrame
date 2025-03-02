namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AccreditToken", "��Ȩ��Ϣ��")]
    public class AccreditTokenModel
    {
        [SugarColumn(IsPrimaryKey = true, Length = 32, ColumnDataType = "varchar", ColumnDescription = "��Ȩ��")]
        public string AccreditId { get; set; }

        [SugarColumn(Length = 32, ColumnDataType = "varchar", ColumnDescription = "������Ȩ��", IsNullable = true)]
        public string PAccreditId { get; set; }

        [SugarColumn(ColumnDataType = "varchar", Length = 50, ColumnDescription = "�û���ʶ��")]
        public string ApplyId { get; set; }

        [SugarColumn(ColumnDataType = "varchar", Length = 50, ColumnDescription = "Ψһ��ʶ��")]
        public string CheckKey { get; set; }

        [SugarColumn(ColumnDataType = "varchar", Length = 50, ColumnDescription = "��ɫ����")]
        public string RoleType { get; set; }

        [SugarColumn(Length = 1000, ColumnDescription = "������ֵ")]
        public string State { get; set; }

        [SugarColumn(ColumnDescription = "������ֵ�汾 ÿ�θ���+1")]
        public int StateVer { get; set; }

        [SugarColumn(ColumnDescription = "��Դ��ȺID")]
        public long RpcMerId { get; set; }

        [SugarColumn(ColumnDescription = "��Դ�ڵ����", Length = 50)]
        public string SysGroup { get; set; }

        [SugarColumn(ColumnDescription = "��Դ�ڵ����", Length = 50)]
        public string SystemType { get; set; }

        [SugarColumn(ColumnDescription = "��Ч��")]
        public DateTime? Expire { get; set; }

        [SugarColumn(ColumnDescription = "��ʱʱ��(�޶��������ʱ��)")]
        public DateTime OverTime { get; set; }

        [SugarColumn(ColumnDescription = "����ʱ��")]
        public DateTime AddTime { get; set; }
    }
}
