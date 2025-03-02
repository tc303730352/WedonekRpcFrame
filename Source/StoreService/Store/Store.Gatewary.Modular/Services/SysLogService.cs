using WeDonekRpc.Model;
using RpcStore.RemoteModel.SysLog;
using RpcStore.RemoteModel.SysLog.Model;
using Store.Gatewary.Modular.Interface;

namespace Store.Gatewary.Modular.Services
{
    internal class SysLogService : ISysLogService
    {
        public SystemLogData GetSysLog(long id)
        {
            return new GetSysLog
            {
                Id = id,
            }.Send();
        }

        public SystemLog[] QuerySysLog(SysLogQuery query, IBasicPage paging, out int count)
        {
            return new QuerySysLog
            {
                Query = query,
                Index = paging.Index,
                Size = paging.Size,
                NextId = paging.NextId,
                SortName = paging.SortName,
                IsDesc = paging.IsDesc
            }.Send(out count);
        }

    }
}
