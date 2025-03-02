namespace RpcExtend.Model.DB
{
    using System;
    using SqlSugar;

    [SugarTable("RpcTraceLog")]
    public class RpcTraceLog
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string TraceId { get; set; }

        public long SpanId { get; set; }

        public long ParentId { get; set; }

        public string Dictate { get; set; }

        public string Show { get; set; }

        public long ServerId { get; set; }

        public string SystemType { get; set; }

        public long RemoteId { get; set; }

        public int RegionId { get; set; }

        public string ReturnRes { get; set; }

        public string Args { get; set; }

        public string MsgType { get; set; }

        public byte StageType { get; set; }

        public DateTime BeginTime { get; set; }

        public int Duration { get; set; }
    }
}
