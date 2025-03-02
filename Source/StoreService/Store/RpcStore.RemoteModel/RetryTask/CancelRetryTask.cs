using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.RetryTask
{
    /// <summary>
    /// 取消任务
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class CancelRetryTask : RpcRemote
    {
        public long[] Ids
        {
            get;
            set;
        }
    }
}
