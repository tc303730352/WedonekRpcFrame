using RpcStore.DAL;
using RpcStore.Model.AutoRetryTask;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Collect.lmpl
{
    internal class AutoRetryTaskCollect : IAutoRetryTaskCollect
    {
        private readonly IAutoRetryTaskDAL _RetryTaskDAL;

        public AutoRetryTaskCollect (IAutoRetryTaskDAL retryTaskDAL)
        {
            this._RetryTaskDAL = retryTaskDAL;
        }
        public AutoRetryTaskModel Get (long id)
        {
            AutoRetryTaskModel task = this._RetryTaskDAL.Get(id);
            if (task == null)
            {
                throw new ErrorException("rpc.retry.task.not.find");
            }
            return task;
        }
        public RetryTaskState GetTaskState (long id)
        {
            RetryTaskState task = this._RetryTaskDAL.GetTaskState(id);
            if (task == null)
            {
                throw new ErrorException("rpc.retry.task.not.find");
            }
            return task;
        }
        public RetryTaskState[] GetTaskStates (long[] ids)
        {
            return this._RetryTaskDAL.GetTaskStates(ids);
        }

        public RetryTask[] Query (RetryTaskQuery query, IBasicPage paging, out int count)
        {
            return this._RetryTaskDAL.Query(query, paging, out count);
        }

    }
}
