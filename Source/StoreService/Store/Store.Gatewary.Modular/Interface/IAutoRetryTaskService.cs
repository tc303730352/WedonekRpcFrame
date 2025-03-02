using RpcStore.RemoteModel.RetryTask.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Interface
{
    public interface IAutoRetryTaskService
    {
        void Add ();
        void Cancel (long[] ids);
        RetryTaskDetailed Get (long id);
        PagingResult<RetryTaskDatum> Query (RetryTaskQuery query, IBasicPage paging);
        void Reset (long serverId, string identityId);
    }
}