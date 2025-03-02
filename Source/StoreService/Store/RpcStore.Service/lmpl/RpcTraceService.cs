using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.Trace.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class RpcTraceService : IRpcTraceService
    {
        private readonly IRpcTraceCollect _Trace;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerCollect _Server;
        private readonly IServerRegionCollect _ServerRegion;
        private readonly IRpcMerCollect _RpcMer;
        public RpcTraceService (IRpcTraceCollect trace,
                IServerTypeCollect serverType,
                IRpcMerCollect rpcMer,
                IServerCollect server,
                IServerRegionCollect region)
        {
            this._Server = server;
            this._RpcMer = rpcMer;
            this._ServerRegion = region;
            this._ServerType = serverType;
            this._Trace = trace;
        }
        public PagingResult<RpcTrace> Query (TraceQuery query, IBasicPage paging)
        {
            RpcTraceModel[] list = this._Trace.Query(query, paging, out int count);
            if (list.Length == 0)
            {
                return new PagingResult<RpcTrace>();
            }
            Dictionary<string, string> types = this._ServerType.GetNames(list.Distinct(c => c.SystemType));
            Dictionary<int, string> regions = this._ServerRegion.GetNames(list.Distinct(c => c.RegionId));
            Dictionary<long, string> rpcMer = this._RpcMer.GetNames(list.Distinct(c => c.RpcMerId));
            Dictionary<long, string> servers = this._Server.GetNames(list.Distinct(c => c.ServerId));
            RpcTrace[] traces = list.ConvertMap<RpcTraceModel, RpcTrace>((a, b) =>
            {
                b.ServerName = servers.GetValueOrDefault(a.ServerId);
                b.SystemTypeName = types.GetValueOrDefault(a.SystemType);
                b.Region = regions.GetValueOrDefault(a.RegionId);
                b.RpcMer = rpcMer.GetValueOrDefault(a.RpcMerId);
                return b;
            });
            return new PagingResult<RpcTrace>(traces, count);
        }
    }
}
