namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("RpcTraceList", "主链路日志表")]
    public class RpcTraceModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "日志ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "链路TraceId", Length = 32, ColumnDataType = "varchar")]
        public string TraceId { get; set; }

        [SugarColumn(ColumnDescription = "执行的指令或Uri", ColumnDataType = "varchar", Length = 1000)]
        public string Dictate { get; set; }

        [SugarColumn(ColumnDescription = "备注说明", Length = 50, IsNullable = true)]
        public string Show { get; set; }


        [SugarColumn(ColumnDescription = "记录产生时间")]
        public DateTime BeginTime { get; set; }

        [SugarColumn(ColumnDescription = "耗时")]
        public int Duration { get; set; }

        [SugarColumn(ColumnDescription = "发起的服务节点")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "服务类别值", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }


        [SugarColumn(ColumnDescription = "机房ID")]
        public int RegionId { get; set; }

        [SugarColumn(ColumnDescription = "集群ID")]
        public long RpcMerId { get; set; }
    }
}
