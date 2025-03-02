using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.BroadcastErrorLog
{
    /// <summary>
    /// 获取广播错误日志
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetBroadcastErrorLog : RpcRemote<BroadcastLog>
    {
        public long Id
        {
            get;
            set;
        }
    }
}
