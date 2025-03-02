using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.BroadcastErrorLog
{
    /// <summary>
    /// 查询广播错误日志
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryBroadcastLog : BasicPage<BroadcastLogDatum>
    {
        /// <summary>
        /// 查询参数
        /// </summary>
        public BroadcastErrorQuery Query
        {
            get;
            set;
        }
        /// <summary>
        /// 返回错误语言类型
        /// </summary>
        public string Lang
        {
            get;
            set;
        } = "zh";
    }
}
