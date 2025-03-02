using WeDonekRpc.Client;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using RpcStore.Collect;
using RpcStore.Model.ExtendDB;
using RpcStore.Model.SysError;
using RpcStore.RemoteModel.SysLog.Model;
using RpcStore.Service.Interface;

namespace RpcStore.Service.lmpl
{
    internal class SysLogService : ISysLogService
    {
        private readonly ISystemLogCollect _ErrorLog;
        private readonly IServerTypeCollect _ServerType;
        private readonly IServerCollect _Server;
        private readonly IRpcMerCollect _RpcMer;

        public SysLogService (ISystemLogCollect errorLog,
            IServerTypeCollect serverType,
            IServerCollect server,
            IRpcMerCollect rpcMer)
        {
            this._ErrorLog = errorLog;
            this._ServerType = serverType;
            this._Server = server;
            this._RpcMer = rpcMer;
        }

        public SystemLogData GetSysLog (long id)
        {
            SystemErrorLogModel log = this._ErrorLog.Get(id);
            SystemLogData data = log.ConvertMap<SystemErrorLogModel, SystemLogData>();
            data.RpcMerName = this._RpcMer.GetName(data.RpcMerId);
            data.SystemTypeName = this._ServerType.GetName(data.SystemType);
            data.ServerName = this._Server.GetName(data.ServerId);
            return data;
        }

        public PagingResult<SystemLog> Query (SysLogQuery query, IBasicPage paging)
        {
            SysErrorLog[] logs = this._ErrorLog.Query(query, paging, out int count);
            if (logs.Length == 0)
            {
                return new PagingResult<SystemLog>();
            }
            Dictionary<string, string> types = this._ServerType.GetNames(logs.Distinct(a => a.SystemType));
            Dictionary<long, string> servers = this._Server.GetNames(logs.Distinct(a => a.ServerId));
            SystemLog[] list = logs.ConvertMap<SysErrorLog, SystemLog>((a, b) =>
            {
                b.ServerName = servers.GetValueOrDefault(a.ServerId);
                b.SystemTypeName = types.GetValueOrDefault(a.SystemType);
                return b;
            });
            return new PagingResult<SystemLog>(list, count);
        }
    }
}
