using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.TraceLog;
using RpcStore.RemoteModel.TraceLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class TraceLogService : ITraceLogService
    {
        private readonly ITraceLogCollect _Trace;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerRegionCollect _ServerRegion;
        private readonly IServerCollect _Server;
        public TraceLogService (ITraceLogCollect trace,
                IServerTypeCollect serverType,
                IServerCollect server,
                IServerRegionCollect region)
        {
            this._Server = server;
            this._ServerRegion = region;
            this._ServerType = serverType;
            this._Trace = trace;
        }
        public TraceLogDatum Get (long id)
        {
            RpcTraceLogModel trace = this._Trace.Get(id);
            TraceLogDatum log = trace.ConvertMap<RpcTraceLogModel, TraceLogDatum>();
            log.ServerName = this._Server.GetName(log.ServerId);
            return log;
        }
        private Dictionary<string, string> _SystemType;
        private Dictionary<int, string> _RegionName;
        private Dictionary<long, string> _ServerName;
        public TraceLog[] Gets (string traceId)
        {
            RpcTraceLog[] list = this._Trace.Gets(traceId);
            if (list.Length == 0)
            {
                return null;
            }
            this._SystemType = this._ServerType.GetNames(list.Distinct(a => a.SystemType));
            this._RegionName = this._ServerRegion.GetNames(list.Distinct(a => a.RegionId));
            this._ServerName = this._Server.GetNames(list.Where(a => a.RemoteId != 0).Select(a => a.RemoteId).Distinct().ToArray());
            return list.ConvertMap<RpcTraceLog, TraceLog>(a => a.ParentId == 0, (a, b) =>
             {
                 b.SystemTypeName = this._SystemType.GetValueOrDefault(a.SystemType);
                 b.Region = this._RegionName.GetValueOrDefault(a.RegionId);
                 if (b.RemoteId != 0)
                 {
                     b.RemoteServerName = this._ServerName.GetValueOrDefault(a.RemoteId);
                 }
                 b.Children = this._GetChildren(a.SpanId, list);
                 return b;
             });
        }
        private TraceLog[] _GetChildren (long spanId, RpcTraceLog[] list)
        {
            return list.ConvertMap<RpcTraceLog, TraceLog>(a => a.ParentId == spanId, (a, b) =>
            {
                b.SystemTypeName = this._SystemType.GetValueOrDefault(a.SystemType);
                b.Region = this._RegionName.GetValueOrDefault(a.RegionId);
                if (b.RemoteId != 0)
                {
                    b.RemoteServerName = this._ServerName.GetValueOrDefault(a.RemoteId);
                }
                b.Children = this._GetChildren(a.SpanId, list);
                return b;
            });
        }
    }
}
