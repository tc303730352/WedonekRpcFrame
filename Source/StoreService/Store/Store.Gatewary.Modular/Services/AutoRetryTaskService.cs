using RpcStore.RemoteModel.Error;
using RpcStore.RemoteModel.Error.Model;
using RpcStore.RemoteModel.RetryTask;
using RpcStore.RemoteModel.RetryTask.Model;
using Store.Gatewary.Modular.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;

namespace Store.Gatewary.Modular.Services
{
    internal class AutoRetryTaskService : IAutoRetryTaskService
    {
        private readonly IAutoRetryService _RetryService;

        public AutoRetryTaskService (IAutoRetryService retryService)
        {
            this._RetryService = retryService;
        }
        public void Add ()
        {
            string error = "3406865759208085";
            this._RetryService.AddTask<SetErrorMsg>(new SetErrorMsg
            {
                Datum = new ErrorSet
                {
                    ErrorId = 3406865759208085,
                    Lang = "zh",
                    Msg = "系统繁忙，清稍后重试!"
                }
            }, error, "设置错误文字描述信息");
        }
        public void Cancel (long[] ids)
        {
            new CancelRetryTask
            {
                Ids = ids
            }.Send();
        }
        public RetryTaskDetailed Get (long id)
        {
            return new GetRetryTask
            {
                Id = id
            }.Send();
        }
        public PagingResult<RetryTaskDatum> Query (RetryTaskQuery query, IBasicPage paging)
        {
            return new QueryRetryTask
            {
                Size = paging.Size,
                SortName = paging.SortName,
                Index = paging.Index,
                IsDesc = paging.IsDesc,
                NextId = paging.NextId,
                Query = query
            }.Send();
        }
        public void Reset (long serverId, string identityId)
        {
            this._RetryService.Restart(serverId, identityId);
        }

    }
}
