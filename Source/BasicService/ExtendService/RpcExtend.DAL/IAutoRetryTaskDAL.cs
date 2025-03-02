using RpcExtend.Model.DB;
using RpcExtend.Model.RetryTask;

namespace RpcExtend.DAL
{
    public interface IAutoRetryTaskDAL
    {
        RetryTask GetTask (string IdentityId);
        RetryTaskBase Get (string identityId);
        void Add (AutoRetryTaskModel add);
        void Cancel (long id);
        RetryTask[] LoadRetryTask (long serverId);
        void LockTask (long[] ids);
        void RetryFail (long id, string error);
        void RetryResult (RetryTaskResult[] results);
        void RetryResult (RetryTaskResult result);
        bool CheckIsReg (string identityId);
        void Restart (long id, RestartTask set);
    }
}