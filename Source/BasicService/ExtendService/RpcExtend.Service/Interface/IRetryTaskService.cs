using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Interface
{
    public interface IRetryTaskService
    {
        void Add (RetryTaskAdd add, MsgSource source);
        void Cancel (string identityId);
        RetryResult GetResult (string identityId);
        void Restart (string IdentityId);
    }
}