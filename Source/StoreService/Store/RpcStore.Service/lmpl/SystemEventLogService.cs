using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysEventLog;
using RpcStore.RemoteModel.SysEventLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class SystemEventLogService : ISystemEventLogService
    {
        private readonly ISystemEventLogCollect _SysEventLog;

        private readonly IServerCollect _Server;
        private readonly IServerRegionCollect _Region;
        private readonly IServerTypeCollect _ServerType;

        public SystemEventLogService (ISystemEventLogCollect sysEventLog,
            IServerCollect server,
            IServerRegionCollect region,
            IServerTypeCollect serverType)
        {
            this._SysEventLog = sysEventLog;
            this._Server = server;
            this._Region = region;
            this._ServerType = serverType;
        }
        public SysEventLogData Get (long id)
        {
            SystemEventLogModel log = this._SysEventLog.Get(id);
            SysEventLogData data = log.ConvertMap<SystemEventLogModel, SysEventLogData>();
            if (data.ServerId != 0)
            {
                data.ServerName = this._Server.GetName(data.ServerId);
            }
            if (data.RegionId != 0)
            {
                data.Region = this._Region.GetName(data.RegionId);
            }
            data.SystemType = this._ServerType.GetName(data.SystemTypeId);
            return data;
        }
        public PagingResult<SystemEventLogDto> Query (SysEventLogQuery query, IBasicPage paging)
        {
            SystemEventLog[] logs = this._SysEventLog.Query(query, paging, out int count);
            if (logs.IsNull())
            {
                return new PagingResult<SystemEventLogDto>(count);
            }
            Dictionary<long, string> sysType = this._ServerType.GetNames(logs.Distinct(a => a.SystemTypeId));
            Dictionary<long, string> servers = this._Server.GetNames(logs.Distinct(a => a.ServerId));
            Dictionary<int, string> regions = this._Region.GetNames(logs.Distinct(a => a.RegionId));
            SystemEventLogDto[] dtos = logs.ConvertMap<SystemEventLog, SystemEventLogDto>((a, b) =>
            {
                b.SystemType = sysType.GetValueOrDefault(a.SystemTypeId);
                b.ServerName = servers.GetValueOrDefault(a.ServerId);
                b.Region = regions.GetValueOrDefault(a.RegionId);
                return b;
            });
            return new PagingResult<SystemEventLogDto>(dtos, count);
        }
    }
}
