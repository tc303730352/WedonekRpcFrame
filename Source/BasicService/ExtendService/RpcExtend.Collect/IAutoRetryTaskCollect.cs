using RpcExtend.Model.RetryTask;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcExtend.Collect
{
    public interface IAutoRetryTaskCollect
    {
        RetryTaskBase Get (string identityId);
        RetryTask Add (RetryTaskAdd add, MsgSource source);
        RetryTask[] LoadRetryTask ();
        void LockTask (long[] ids);
        void RetryFail (RetryTask task, string error);
        void RetryResult (RetryTaskResult[] results);
        void RetryResult (RetryTaskResult result);
        bool Cancel (RetryTaskBase task);
        RetryTask GetTask (string id);
    }
}