using WeDonekRpc.Client;
using RpcStore.RemoteModel.Trace.Model;

namespace RpcStore.RemoteModel.Trace
{
    /// <summary>
    /// 查询链路信息
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryTrace : BasicPage<RpcTrace>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public TraceQuery Query
        {
            get;
            set;
        }
    }
}
