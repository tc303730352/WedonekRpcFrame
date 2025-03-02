using WeDonekRpc.Model;
using WeDonekRpc.Model.Model;

namespace RpcSync.Collect.Model
{
    public class BroadcastBody
    {
        public BroadcastDatum Datum { get; set; }
        public MsgSource Source { get; set; }
        public TrackSpan Track { get; set; }
        public long RpcMerId { get; set; }
        public string[] Dictate { get; set; }
        public long[] ServerId { get; set; }
        public bool IsExclude { get; set; }
    }
}
