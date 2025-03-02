using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace RpcStore.Service.Interface
{
    public interface IAutoRetryTaskService
    {
        void Cancel (long[] ids);
        RetryTaskDetailed Get (long id);
        PagingResult<RetryTaskDatum> Query (RetryTaskQuery query, IBasicPage paging);

    }
}