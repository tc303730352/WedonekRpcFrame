namespace DataBasicCli.RpcExtendService
{
    using SqlSugar;

    [SugarTable("Idgenerator", "Yitter.IdGenerator雪花标识算法WorkId分配表")]
    public class IdgeneratorModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "分配的WorkId")]
        public int WorkId { get; set; }

        [SugarColumn(ColumnDescription = "服务节点类型ID")]
        public long SystemTypeId { get; set; }

        /// <summary>
        /// 服务节点的物理MAC地址
        /// </summary>
        [SugarColumn(ColumnDescription = "服务节点的物理MAC地址", Length = 17, ColumnDataType = "varchar", IsNullable = false)]
        public string Mac { get; set; }

        [SugarColumn(ColumnDescription = "服务编号")]
        public int ServerIndex { get; set; }
    }
}
