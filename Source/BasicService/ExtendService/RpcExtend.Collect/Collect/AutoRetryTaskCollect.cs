using RpcExtend.DAL;
using RpcExtend.Model.DB;
using RpcExtend.Model.RetryTask;
using WeDonekRpc.Client;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcExtend.Collect.Collect
{
    internal class AutoRetryTaskCollect : IAutoRetryTaskCollect
    {
        private readonly IAutoRetryTaskDAL _BasicDAL;

        public AutoRetryTaskCollect (IAutoRetryTaskDAL basicDAL)
        {
            this._BasicDAL = basicDAL;
        }
        public RetryTaskBase Get (string identityId)
        {
            RetryTaskBase task = this._BasicDAL.Get(identityId);
            if (task == null)
            {
                throw new ErrorException("rpc.retry.task.not.find");
            }
            return task;
        }
        public RetryTask Add (RetryTaskAdd add, MsgSource source)
        {
            if (this._BasicDAL.CheckIsReg(add.IdentityId))
            {
                throw new ErrorException("rpc.retry.task.repeat");
            }
            AutoRetryTaskModel data = new AutoRetryTaskModel
            {
                RpcMerId = source.RpcMerId,
                RegionId = source.RegionId,
                ServerId = source.ServerId,
                Show = add.Show,
                IdentityId = add.IdentityId,
                SystemType = source.SystemType,
                RegServiceId = RpcClient.ServerId,
                RetryConfig = add.RetryConfig,
                SendBody = add.SendBody,
                NextRetryTime = add.RetryConfig.GetOneRetryTime()
            };
            this._BasicDAL.Add(data);
            return data.ConvertMap<AutoRetryTaskModel, RetryTask>();
        }

        public RetryTask[] LoadRetryTask ()
        {
            return this._BasicDAL.LoadRetryTask(RpcClient.ServerId);
        }
        public void LockTask (long[] ids)
        {
            this._BasicDAL.LockTask(ids);
        }

        public void RetryFail (RetryTask task, string error)
        {
            this._BasicDAL.RetryFail(task.Id, error);
        }

        public void RetryResult (RetryTaskResult[] results)
        {
            this._BasicDAL.RetryResult(results);
        }
        public void RetryResult (RetryTaskResult result)
        {
            this._BasicDAL.RetryResult(result);
        }
        public bool Cancel (RetryTaskBase task)
        {
            if (task.Status == AutoRetryTaskStatus.已取消)
            {
                return false;
            }
            else if (task.Status != AutoRetryTaskStatus.待重试)
            {
                throw new ErrorException("rpc.retry.task.status.error");
            }
            this._BasicDAL.Cancel(task.Id);
            return true;
        }

        public RetryTask GetTask (string IdentityId)
        {
            RetryTask task = this._BasicDAL.GetTask(IdentityId);
            if (task == null)
            {
                throw new ErrorException("rpc.retry.task.not.find");
            }
            return task;
        }
    }
}
