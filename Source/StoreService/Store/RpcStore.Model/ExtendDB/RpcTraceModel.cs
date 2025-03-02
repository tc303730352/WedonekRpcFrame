namespace RpcStore.Model.ExtendDB
{
    using System;
    using SqlSugar;

    [SugarTable("RpcTraceList")]
    public class RpcTraceModel
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Id { get; set; }

        public string TraceId { get; set; }

        public string Dictate { get; set; }

        public string Show { get; set; }

        public DateTime BeginTime { get; set; }

        public int Duration { get; set; }

        public long ServerId { get; set; }

        public string SystemType { get; set; }

        public int RegionId { get; set; }

        public long RpcMerId { get; set; }
    }
}
