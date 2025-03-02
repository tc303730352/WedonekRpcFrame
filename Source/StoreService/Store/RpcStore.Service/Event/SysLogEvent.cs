using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.SysLog;
using RpcStore.RemoteModel.SysLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    internal class SysLogEvent : IRpcApiService
    {
        public ISysLogService _Service;
        public SysLogEvent(ISysLogService service)
        {
            _Service = service;
        }

        public SystemLogData GetSysLog(GetSysLog obj)
        {
            return _Service.GetSysLog(obj.Id);
        }

        public PagingResult<SystemLog> QuerySysLog(QuerySysLog query)
        {
            return _Service.Query(query.Query, query.ToBasicPage());
        }
    }
}
