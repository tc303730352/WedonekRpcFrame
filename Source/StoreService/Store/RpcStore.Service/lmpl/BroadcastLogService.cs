using RpcStore.Collect;
using RpcStore.Model.BroadcastErrorLog;
using RpcStore.Model.ExtendDB;
using RpcStore.RemoteModel.BroadcastErrorLog.Model;
using RpcStore.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcStore.Service.lmpl
{
    internal class BroadcastLogService : IBroadcastLogService
    {
        private readonly IBroadcastLogCollect _Broadcast;
        private readonly IRpcMerCollect _RpcMer;
        private readonly IServerCollect _Server;
        private readonly IServerTypeCollect _ServerType;
        private readonly IContainerGroupCollect _ContainerGroup;
        private readonly IRegionService _Region;
        private readonly IErrorLangMsgCollect _ErrorMsg;
        public BroadcastLogService (IBroadcastLogCollect broadcast,
                IServerCollect server,
                IRpcMerCollect rpcMer,
                IServerTypeCollect serverType,
                IContainerGroupCollect containerGroup,
                IRegionService region,
                IErrorLangMsgCollect errorMsg)
        {
            this._Region = region;
            this._ContainerGroup = containerGroup;
            this._RpcMer = rpcMer;
            this._Server = server;
            this._ServerType = serverType;
            this._ErrorMsg = errorMsg;
            this._Broadcast = broadcast;
        }
        public BroadcastLog Get (long id)
        {
            BroadcastErrorLogModel log = this._Broadcast.Get(id);
            MsgSource source = log.MsgSource;
            MsgSourceDto dto = new MsgSourceDto
            {
                VerNum = source.VerNum.FormatVerNum(),
                ServerName = this._Server.GetName(source.ServerId),
                SystemTypeName = this._ServerType.GetName(source.SystemType),
                SystemType = source.SystemType,
                ContGroup = source.ContGroup.HasValue ? this._ContainerGroup.GetName(source.ContGroup.Value) : null,
                Region = this._Region.GetName(source.RegionId),
                RpcMer = this._RpcMer.GetName(source.RpcMerId)
            };
            return new BroadcastLog
            {
                ServerId = log.ServerId,
                MsgSource = dto,
                SysTypeVal = log.SysTypeVal,
                AddTime = log.AddTime,
                BroadcastType = log.BroadcastType,
                Error = log.Error,
                Id = log.Id,
                MsgBody = log.MsgBody,
                MsgKey = log.MsgKey,
                RouteKey = log.RouteKey,
                RpcMerId = log.RpcMerId
            };
        }

        public PagingResult<BroadcastLogDatum> Query (BroadcastErrorQuery query, string lang, IBasicPage paging)
        {
            BroadcastErrorLog[] logs = this._Broadcast.Query(query, paging, out int count);
            if (logs.Length == 0)
            {
                return new PagingResult<BroadcastLogDatum>();
            }
            Dictionary<string, string> errors = this._ErrorMsg.GetErrorDic(logs.Distinct(c => c.Error), lang);
            Dictionary<long, string> mers = this._RpcMer.GetNames(logs.Where(a => a.RpcMerId != 0).Select(c => c.RpcMerId).Distinct().ToArray());
            List<long> serverId = new List<long>(logs.Length * 2);
            logs.ForEach(c =>
            {
                if (c.ServerId != 0)
                {
                    serverId.Add(c.ServerId);
                }
                serverId.Add(c.SourceId);
            });
            Dictionary<long, string> services = this._Server.GetNames(serverId.Distinct().ToArray());
            Dictionary<string, string> types = this._ServerType.GetNames(logs.Where(a => !a.SysTypeVal.IsNull()).Select(c => c.SysTypeVal).Distinct().ToArray());
            BroadcastLogDatum[] datas = logs.ConvertMap<BroadcastErrorLog, BroadcastLogDatum>((a, b) =>
            {
                b.ErrorText = errors.GetValueOrDefault(a.Error);
                if (a.RpcMerId != 0)
                {
                    b.RpcMer = mers.GetValueOrDefault(a.RpcMerId);
                }
                if (a.ServerId != 0)
                {
                    b.ServerName = services.GetValueOrDefault(a.ServerId);
                }
                b.SourceServer = services.GetValueOrDefault(a.SourceId);
                if (a.SysTypeVal.IsNotNull())
                {
                    b.SystemTypeName = types.GetValueOrDefault(a.SysTypeVal);
                }
                return b;
            });
            return new PagingResult<BroadcastLogDatum>(datas, count);
        }
    }
}
