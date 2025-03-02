using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.TraceLog
{
    /// <summary>
    /// 获取单个链路下的日志
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetTraceByTraceId : RpcRemoteArray<Model.TraceLog>
    {
        /// <summary>
        /// 链路ID
        /// </summary>
        public string TraceId { get; set; }
    }
}
