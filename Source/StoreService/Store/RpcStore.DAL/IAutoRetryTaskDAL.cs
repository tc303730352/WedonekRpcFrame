using RpcStore.Model.AutoRetryTask;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Model;

namespace RpcStore.DAL
{
    public interface IAutoRetryTaskDAL
    {
        AutoRetryTaskModel Get ( long id );
        RetryTaskState GetTaskState ( long id );
        RetryTaskState[] GetTaskStates ( long[] ids );

        RetryTask[] Query ( RetryTaskQuery query, IBasicPage paging, out int count );
    }
}