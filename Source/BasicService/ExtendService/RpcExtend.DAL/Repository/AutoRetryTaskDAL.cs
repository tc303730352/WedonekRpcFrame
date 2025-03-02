using RpcExtend.Model.DB;
using RpcExtend.Model.RetryTask;
using WeDonekRpc.Client;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.SqlSugar;

namespace RpcExtend.DAL.Repository
{
    internal class AutoRetryTaskDAL : IAutoRetryTaskDAL
    {
        private readonly IRepository<AutoRetryTaskModel> _BasicDAL;
        public AutoRetryTaskDAL (IRepository<AutoRetryTaskModel> dal)
        {
            this._BasicDAL = dal;
        }
        public RetryTask GetTask (string IdentityId)
        {
            return this._BasicDAL.Get<RetryTask>(a => a.IdentityId == IdentityId);
        }
        public RetryTaskBase Get (string identityId)
        {
            return this._BasicDAL.Get<RetryTaskBase>(a => a.IdentityId == identityId);
        }
        public void Add (AutoRetryTaskModel add)
        {
            add.Id = IdentityHelper.CreateId();
            add.AddTime = DateTime.Now;
            add.Status = AutoRetryTaskStatus.待重试;
            this._BasicDAL.Insert(add);
        }
        public void Cancel (long id)
        {
            if (!this._BasicDAL.Update(a => a.Status == AutoRetryTaskStatus.已取消, a => a.Id == id && a.Status == AutoRetryTaskStatus.待重试))
            {
                throw new ErrorException("rpc.retry.task.cancel.fail");
            }
        }
        public RetryTask[] LoadRetryTask (long serverId)
        {
            return this._BasicDAL.Gets<RetryTask>(a => a.RegServiceId == serverId && a.Status == AutoRetryTaskStatus.待重试);
        }
        public void LockTask (long[] ids)
        {
            if (!this._BasicDAL.Update(a => a.IsLock == true, a => ids.Contains(a.Id)))
            {
                throw new ErrorException("rpc.retry.task.lock.fail");
            }
        }

        public void RetryFail (long id, string error)
        {
            if (!this._BasicDAL.Update(a => new AutoRetryTaskModel
            {
                ComplateTime = DateTime.Now,
                Status = AutoRetryTaskStatus.已重试失败,
                ErrorCode = error
            }, a => a.Id == id))
            {
                throw new ErrorException("rpc.retry.task.set.fail");
            }
        }

        public void RetryResult (RetryTaskResult[] results)
        {
            AutoRetryTaskModel[] sets = results.ConvertMap<RetryTaskResult, AutoRetryTaskModel>();
            if (!this._BasicDAL.Update(sets, "Status", "NextRetryTime", "RetryNum", "ErrorCode", "ComplateTime", "IsLock"))
            {
                throw new ErrorException("rpc.retry.task.set.fail");
            }
        }
        public void RetryResult (RetryTaskResult result)
        {
            if (!this._BasicDAL.Update(a => new AutoRetryTaskModel
            {
                IsLock = result.IsLock,
                Status = result.Status,
                ErrorCode = result.ErrorCode,
                ComplateTime = result.ComplateTime,
                RetryNum = result.RetryNum,
                NextRetryTime = result.NextRetryTime,
            }, a => a.Id == result.Id))
            {
                throw new ErrorException("rpc.retry.task.set.fail");
            }
        }

        public bool CheckIsReg (string identityId)
        {
            return this._BasicDAL.IsExist(c => c.IdentityId == identityId);
        }

        public void Restart (long id, RestartTask set)
        {
            if (!this._BasicDAL.Update(set, a => a.Id == id))
            {
                throw new ErrorException("rpc.retry.task.set.fail");
            }
        }
    }
}
