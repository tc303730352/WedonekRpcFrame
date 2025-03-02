using WeDonekRpc.Client;
using RpcStore.RemoteModel.TraceLog.Model;

namespace RpcStore.RemoteModel.TraceLog
{
    /// <summary>
    /// 获取单个链路日志详细
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetTraceLog : RpcRemote<TraceLogDatum>
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public long Id
        {
            get;
            set;
        }
    }
}
