namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("AccreditToken", "授权信息表")]
    public class AccreditTokenModel
    {
        [SugarColumn(IsPrimaryKey = true, Length = 32, ColumnDataType = "varchar", ColumnDescription = "授权码")]
        public string AccreditId { get; set; }

        [SugarColumn(Length = 32, ColumnDataType = "varchar", ColumnDescription = "父级授权码", IsNullable = true)]
        public string PAccreditId { get; set; }

        [SugarColumn(ColumnDataType = "varchar", Length = 50, ColumnDescription = "用户标识码")]
        public string ApplyId { get; set; }

        [SugarColumn(ColumnDataType = "varchar", Length = 50, ColumnDescription = "唯一标识码")]
        public string CheckKey { get; set; }

        [SugarColumn(ColumnDataType = "varchar", Length = 50, ColumnDescription = "角色类型")]
        public string RoleType { get; set; }

        [SugarColumn(Length = 1000, ColumnDescription = "附带的值")]
        public string State { get; set; }

        [SugarColumn(ColumnDescription = "附带的值版本 每次更新+1")]
        public int StateVer { get; set; }

        [SugarColumn(ColumnDescription = "来源集群ID")]
        public long RpcMerId { get; set; }

        [SugarColumn(ColumnDescription = "来源节点组别", Length = 50)]
        public string SysGroup { get; set; }

        [SugarColumn(ColumnDescription = "来源节点类别", Length = 50)]
        public string SystemType { get; set; }

        [SugarColumn(ColumnDescription = "有效期")]
        public DateTime? Expire { get; set; }

        [SugarColumn(ColumnDescription = "超时时间(无动作清除的时间)")]
        public DateTime OverTime { get; set; }

        [SugarColumn(ColumnDescription = "创建时间")]
        public DateTime AddTime { get; set; }
    }
}
