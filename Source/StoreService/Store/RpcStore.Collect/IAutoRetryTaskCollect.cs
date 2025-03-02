using RpcStore.Model.AutoRetryTask;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcStore.Collect
{
    public interface IAutoRetryTaskCollect
    {
        AutoRetryTaskModel Get (long id);
        RetryTaskState[] GetTaskStates (long[] ids);

        RetryTaskState GetTaskState (long id);
        RetryTask[] Query (RetryTaskQuery query, IBasicPage paging, out int count);
    }
}