using RpcExtend.Collect;
using RpcExtend.Model.RetryTask;
using RpcExtend.Service.AutoRetry;
using RpcExtend.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.RetryTask;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Service
{
    internal class RetryTaskService : IRetryTaskService
    {
        private readonly IAutoRetryTaskCollect _RetryTask;
        private readonly IRemoteSendService _Send;
        public RetryTaskService (IAutoRetryTaskCollect retryTask, IRemoteSendService send)
        {
            this._Send = send;
            this._RetryTask = retryTask;
        }
        public void Cancel (string identityId)
        {
            RetryTaskBase task = this._RetryTask.Get(identityId);
            if (!this._RetryTask.Cancel(task))
            {
                return;
            }
            else if (task.RegServiceId == RpcClient.ServerId)
            {
                AutoRetryTaskService.Cancel(task.Id);
            }
            else
            {
                this._Send.Send<CloseTask>(task.RegServiceId, new CloseTask
                {
                    TaskId = task.Id,
                });
            }

        }
        public void Add (RetryTaskAdd add, MsgSource source)
        {
            RetryTask task = this._RetryTask.Add(add, source);
            if (!AutoRetryTaskService.Add(task))
            {
                this._RetryTask.RetryFail(task, "rpc.retry.exec.expire");
            }
        }

        public RetryResult GetResult (string identityId)
        {
            RetryTaskBase task = this._RetryTask.Get(identityId);
            return task.ConvertMap<RetryTaskBase, RetryResult>();
        }

        public void Restart (string IdentityId)
        {
            RetryTask task = this._RetryTask.GetTask(IdentityId);
            AutoRetryTaskService.RunTask(task);
        }
    }
}
