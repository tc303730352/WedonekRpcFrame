namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("ResourceModular", "资源模块")]
    public class ResourceModularModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "模块ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "模块KEY", Length = 32, ColumnDataType = "varchar")]
        public string ModularKey { get; set; }
        [SugarColumn(ColumnDescription = "集群Id")]
        public long RpcMerId { get; set; }
        [SugarColumn(ColumnDescription = "服务类别值", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }
        [SugarColumn(ColumnDescription = "模块名", Length = 50)]
        public string ModularName { get; set; }
        [SugarColumn(ColumnDescription = "备注", Length = 50, IsNullable = true)]
        public string Remark { get; set; }
        [SugarColumn(ColumnDescription = "资源类型")]
        public byte ResourceType { get; set; }
        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime { get; set; }
    }
}
