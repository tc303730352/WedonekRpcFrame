using RpcExtend.Service.AutoRetry;
using RpcExtend.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.RetryTask;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Event
{
    internal class RetryTaskEvent : IRpcApiService
    {
        private readonly IRetryTaskService _Service;

        public RetryTaskEvent (IRetryTaskService service)
        {
            this._Service = service;
        }
        public void RestartTask (RestartTask obj)
        {
            this._Service.Restart(obj.IdentityId);
        }
        public void AddRetryTask (AddRetryTask add, MsgSource source)
        {
            this._Service.Add(add.Task, source);
        }
        public void CloseTask (CloseTask obj)
        {
            AutoRetryTaskService.Cancel(obj.TaskId);
        }
        public RetryResult GetRetryResult (GetRetryResult obj)
        {
            return this._Service.GetResult(obj.IdentityId);
        }
        public void CancelTask (CancelTask obj)
        {
            this._Service.Cancel(obj.IdentityId);
        }
    }
}
