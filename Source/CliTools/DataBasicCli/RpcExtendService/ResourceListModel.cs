namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("ResourceList", "资源表")]
    public class ResourceListModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "资源ID")]
        public long Id { get; set; }
        [SugarColumn(ColumnDescription = "模块ID")]
        public long ModularId { get; set; }

        [SugarColumn(ColumnDescription = "资源相对路径", ColumnDataType = "varchar", Length = 100)]
        public string ResourcePath { get; set; }

        [SugarColumn(ColumnDescription = "资源绝对路径", ColumnDataType = "varchar", Length = 300)]
        public string FullPath { get; set; }

        [SugarColumn(ColumnDescription = "资源说明", Length = 100)]
        public string ResourceShow { get; set; }

        [SugarColumn(ColumnDescription = "资源状态")]
        public byte ResourceState { get; set; }

        [SugarColumn(ColumnDescription = "应用版本号")]
        public int VerNum { get; set; }

        [SugarColumn(ColumnDescription = "资源版本号")]
        public int ResourceVer { get; set; }

        [SugarColumn(ColumnDescription = "最后更新时间", ColumnDataType = "date")]
        public DateTime? LastTime { get; set; }

        [SugarColumn(ColumnDescription = "添加时间")]
        public DateTime AddTime { get; set; }
    }
}
