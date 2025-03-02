using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.RetryTask
{
    /// <summary>
    /// 获取任务详细
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class GetRetryTask : RpcRemote<RetryTaskDetailed>
    {
        public long Id
        {
            get;
            set;
        }
    }
}
