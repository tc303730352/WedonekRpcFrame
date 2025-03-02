using RpcStore.Collect;
using RpcStore.Model.AutoRetryTask;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.RetryTask.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class AutoRetryTaskService : IAutoRetryTaskService
    {
        private readonly IAutoRetryService _RetryService;
        private readonly IAutoRetryTaskCollect _RetryTask;
        private readonly IServerRegionCollect _Region;
        private readonly IAutoRetryTaskLogCollect _RetryLog;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerCollect _Server;

        public AutoRetryTaskService (IAutoRetryTaskCollect retryTask,
            IServerRegionCollect region,
            IServerTypeCollect serverType,
            IAutoRetryService retryService,
            IServerCollect server,
            IAutoRetryTaskLogCollect retryLog)
        {
            this._RetryService = retryService;
            this._RetryTask = retryTask;
            this._Region = region;
            this._ServerType = serverType;
            this._Server = server;
            this._RetryLog = retryLog;
        }

        public void Cancel (long[] ids)
        {
            RetryTaskState[] states = this._RetryTask.GetTaskStates(ids);
            if (states.IsNull())
            {
                return;
            }
            states = states.FindAll(c => c.Status == AutoRetryTaskStatus.待重试);
            if (states.IsNull())
            {
                return;
            }
            states.ForEach(c =>
            {
                this._RetryService.Cancel(c.RegServiceId, c.IdentityId);
            });
        }

        public RetryTaskDetailed Get (long id)
        {
            AutoRetryTaskModel task = this._RetryTask.Get(id);
            RetryTaskDetailed dto = task.ConvertMap<AutoRetryTaskModel, RetryTaskDetailed>();
            dto.RegionName = this._Region.GetName(dto.RegionId);
            dto.ServerName = this._Server.GetName(dto.ServerId);
            dto.RegService = this._Server.GetName(dto.RegServiceId);
            dto.SystemTypeName = this._ServerType.GetName(dto.SystemType);
            AutoRetryTaskLogModel[] logs = this._RetryLog.GetLogs(task.Id);
            if (!logs.IsNull())
            {
                dto.Logs = logs.ConvertMap<AutoRetryTaskLogModel, RetryTaskLog>();
            }
            return dto;
        }
        public PagingResult<RetryTaskDatum> Query (RetryTaskQuery query, IBasicPage paging)
        {
            RetryTask[] tasks = this._RetryTask.Query(query, paging, out int count);
            if (tasks.IsNull())
            {
                return new PagingResult<RetryTaskDatum>();
            }
            Dictionary<int, string> region = this._Region.GetNames(tasks.Distinct(c => c.RegionId));
            Dictionary<long, string> server = this._Server.GetNames(tasks.Select(c => c.ServerId).Concat(tasks.Select(a => a.RegServiceId)).Distinct().ToArray());
            Dictionary<string, string> serverType = this._ServerType.GetNames(tasks.Distinct(c => c.SystemType));
            RetryTaskDatum[] dtos = tasks.ConvertMap<RetryTask, RetryTaskDatum>();
            dtos.ForEach(c =>
            {
                c.SystemTypeName = serverType.GetValueOrDefault(c.SystemType);
                c.ServerName = server.GetValueOrDefault(c.ServerId);
                c.RegService = server.GetValueOrDefault(c.RegServiceId);
                c.RegionName = region.GetValueOrDefault(c.RegionId);
            });
            return new PagingResult<RetryTaskDatum>(dtos, count);
        }

    }
}
