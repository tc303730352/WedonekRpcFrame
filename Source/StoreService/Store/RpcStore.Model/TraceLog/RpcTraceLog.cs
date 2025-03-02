using RpcStore.RemoteModel;

namespace RpcStore.Model.TraceLog
{
    public class RpcTraceLog
    {
        public long Id { get; set; }

        public long SpanId { get; set; }

        public long ParentId { get; set; }

        /// <summary>
        /// 事件指令
        /// </summary>
        public string Dictate { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Show { get; set; }

        public long ServerId { get; set; }
        public string SystemType { get; set; }

        public long RemoteId { get; set; }
        public int RegionId { get; set; }

        public string MsgType { get; set; }

        public StageType StageType { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public int Duration { get; set; }
    }
}
