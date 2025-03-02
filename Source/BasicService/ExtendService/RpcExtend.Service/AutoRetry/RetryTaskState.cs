using RpcExtend.Collect;
using RpcExtend.Model.RetryTask;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Helper;

namespace RpcExtend.Service.AutoRetry
{
    internal class RetryTaskState
    {
        private volatile AutoRetryTaskStatus _Status;
        public RetryTaskState (RetryTask task)
        {
            this.Id = task.Id;
            this.Task = task;
        }
        public bool Init (long now)
        {
            if (this.Task.NextRetryTime <= now)
            {
                if (this.Task.RetryConfig.End.HasValue && now <= this.Task.RetryConfig.End.Value)
                {
                    this._Status = AutoRetryTaskStatus.已重试失败;
                    return false;
                }
                this.Task.NextRetryTime = this.Task.RetryConfig.GetOneRetryTime();
                this._Status = AutoRetryTaskStatus.待重试;
            }
            return true;
        }
        public RetryTask Task { get; }
        public long Id
        {
            get;
        }
        public AutoRetryTaskStatus Status => this._Status;

        public void Cancel ()
        {
            if (this._Status == AutoRetryTaskStatus.待重试)
            {
                this._Status = AutoRetryTaskStatus.已取消;
            }
        }
        public bool SyncState (RetryTaskResult result)
        {
            if (this._Status == AutoRetryTaskStatus.已取消)
            {
                result.ComplateTime = DateTime.Now;
                result.Status = AutoRetryTaskStatus.已取消;
                return false;
            }
            else if (result.Status == AutoRetryTaskStatus.已重试失败)
            {
                result.RetryNum += 1;
                RetryConfig config = this.Task.RetryConfig;
                if (config.End.HasValue && config.End.Value <= DateTime.Now.ToLong())
                {
                    result.ComplateTime = DateTime.Now;
                }
                else if (!config.StopErrorCode.IsNull() && config.StopErrorCode.Contains(result.ErrorCode))
                {
                    result.ComplateTime = DateTime.Now;
                }
                else if (config.MaxRetry.HasValue && result.RetryNum < config.MaxRetry.Value)
                {
                    result.Status = AutoRetryTaskStatus.待重试;
                    result.NextRetryTime = config.GetRetryTime(result.RetryNum);
                }
                else
                {
                    result.ComplateTime = DateTime.Now;
                }
            }
            this.Task.RetryNum = result.RetryNum;
            this.Task.NextRetryTime = result.NextRetryTime;
            this._Status = result.Status;
            return result.Status == AutoRetryTaskStatus.待重试;
        }

        internal bool ResetTime (long time)
        {
            if (this.Task.RetryConfig.End.HasValue && time <= this.Task.RetryConfig.End.Value)
            {
                this._Status = AutoRetryTaskStatus.已重试失败;
                return false;
            }
            return true;
        }
    }
}
