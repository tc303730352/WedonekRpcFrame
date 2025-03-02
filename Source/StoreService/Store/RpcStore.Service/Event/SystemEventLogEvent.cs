using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using RpcStore.RemoteModel.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.Event
{
    /// <summary>
    /// 系统事件日志
    /// </summary>
    internal class SystemEventLogEvent : IRpcApiService
    {
        private readonly ISystemEventLogService _Service;

        public SystemEventLogEvent (ISystemEventLogService service)
        {
            this._Service = service;
        }
        public SysEventLogData GetSysEventLog (GetSysEventLog obj)
        {
            return this._Service.Get(obj.Id);
        }
        public PagingResult<SystemEventLogDto> QuerySysEventLog (QuerySysEventLog obj)
        {
            return this._Service.Query(obj.Query, obj.ToBasicPage());
        }
    }
}
