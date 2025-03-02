namespace DataBasicCli.RpcExtendService
{
    using SqlSugar;
    /// <summary>
    /// 接口资源屏蔽
    /// </summary>
    [SugarTable("ResourceShield", "资源屏蔽表")]
    public class ResourceShieldModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "资源ID")]
        public long ResourceId { get; set; }

        [SugarColumn(ColumnDescription = "屏蔽key", ColumnDataType = "varchar", Length = 36)]
        public string ShieIdKey { get; set; }
        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId { get; set; }
        [SugarColumn(ColumnDescription = "服务类别值", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }
        [SugarColumn(ColumnDescription = "服务节点ID")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "屏蔽的应用版本号")]
        public int VerNum { get; set; }
        [SugarColumn(ColumnDescription = "排序号")]
        public short SortNum { get; set; }

        [SugarColumn(ColumnDescription = "屏蔽类型")]
        public byte ShieldType { get; set; }

        [SugarColumn(ColumnDescription = "资源相对路径", ColumnDataType = "varchar", Length = 100)]
        public string ResourcePath { get; set; }
        [SugarColumn(ColumnDescription = "过期时间")]
        public long BeOverdueTime { get; set; }
        [SugarColumn(ColumnDescription = "屏蔽说明", Length = 100, IsNullable = true)]
        public string ShieIdShow { get; set; }
    }
}
