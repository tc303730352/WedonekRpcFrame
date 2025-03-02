using AutoTask.Model.DB;
using RpcTaskModel;
using WeDonekRpc.Client;

namespace AutoTask.Collect.LocalEvent
{
    internal class AutoTaskEvent : RpcLocalEvent
    {
        public long TaskId { get; set; }
        public AutoTaskModel Task { get; set; }
        public AutoTaskStatus? NewStatus { get; set; }
    }
}
