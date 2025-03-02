namespace DataBasicCli.RpcExtendService
{
    using System;
    using SqlSugar;

    [SugarTable("RpcTraceLog", "链路日志表")]
    public class RpcTraceLogModel
    {
        [SugarColumn(IsPrimaryKey = true, ColumnDescription = "日志ID")]
        public long Id { get; set; }

        [SugarColumn(ColumnDescription = "链路ID", Length = 32, ColumnDataType = "varchar")]
        public string TraceId { get; set; }

        [SugarColumn(ColumnDescription = "SpanId")]
        public long SpanId { get; set; }

        [SugarColumn(ColumnDescription = "父级SpanId")]
        public long ParentId { get; set; }

        [SugarColumn(ColumnDescription = "执行的指令或Uri", ColumnDataType = "varchar", Length = 1000)]
        public string Dictate { get; set; }

        [SugarColumn(ColumnDescription = "备注说明", Length = 50)]
        public string Show { get; set; }

        [SugarColumn(ColumnDescription = "发起服务节点Id")]
        public long ServerId { get; set; }

        [SugarColumn(ColumnDescription = "服务类别值", Length = 50, ColumnDataType = "varchar")]
        public string SystemType { get; set; }


        [SugarColumn(ColumnDescription = "目的地服务节点Id")]
        public long RemoteId { get; set; }

        [SugarColumn(ColumnDescription = "机房ID")]
        public int RegionId { get; set; }


        [SugarColumn(ColumnDescription = "执行结果")]
        public string ReturnRes { get; set; }

        [SugarColumn(ColumnDescription = "发送参数")]
        public string Args { get; set; }

        [SugarColumn(ColumnDescription = "消息类型", Length = 10, ColumnDataType = "varchar", IsNullable = true)]
        public string MsgType { get; set; }

        [SugarColumn(ColumnDescription = "链路方向")]
        public byte StageType { get; set; }

        [SugarColumn(ColumnDescription = "开始记录时间")]
        public DateTime BeginTime { get; set; }

        [SugarColumn(ColumnDescription = "耗时")]
        public int Duration { get; set; }
    }
}
