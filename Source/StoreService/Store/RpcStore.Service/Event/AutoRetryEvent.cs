using RpcStore.RemoteModel.RetryTask;
using RpcStore.RemoteModel.RetryTask.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;

namespace RpcStore.Service.Event
{
    internal class AutoRetryEvent : IRpcApiService
    {
        private readonly IAutoRetryTaskService _Service;

        public AutoRetryEvent (IAutoRetryTaskService service)
        {
            this._Service = service;
        }

        public RetryTaskDetailed GetRetryTask (GetRetryTask obj)
        {
            return this._Service.Get(obj.Id);
        }
        public void CancelRetryTask (CancelRetryTask obj)
        {
            this._Service.Cancel(obj.Ids);
        }

        public PagingResult<RetryTaskDatum> QueryRetryTask (QueryRetryTask query)
        {
            return this._Service.Query(query.Query, query.ToBasicPage());
        }
    }
}
