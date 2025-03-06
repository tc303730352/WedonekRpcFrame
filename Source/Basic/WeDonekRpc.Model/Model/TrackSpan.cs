namespace WeDonekRpc.Model.Model
{
    public class TrackSpan
    {
        /// <summary>
        /// 高位
        /// </summary>
        public long? HighTraceId;
        /// <summary>
        /// 低位
        /// </summary>
        public long TraceId;

        /// <summary>
        /// 父级跟踪ID
        /// </summary>
        public long? ParentId;

        /// <summary>
        /// 跟踪ID
        /// </summary>
        public long SpanId;

        public bool IsEnd;
        public string ToTraceId ()
        {
            if ( this.HighTraceId.HasValue )
            {
                return string.Concat(this.HighTraceId.Value.ToString("x16"), this.TraceId.ToString("x16"));
            }
            return this.TraceId.ToString("x16");
        }

    }
}
