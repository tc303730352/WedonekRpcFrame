using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Client;

namespace RpcStore.RemoteModel.RetryTask
{
    /// <summary>
    /// 查询重试任务列表
    /// </summary>
    [WeDonekRpc.Model.IRemoteConfig("sys.store.service")]
    public class QueryRetryTask : BasicPage<RetryTaskDatum>
    {
        public RetryTaskQuery Query { get; set; }
    }
}
