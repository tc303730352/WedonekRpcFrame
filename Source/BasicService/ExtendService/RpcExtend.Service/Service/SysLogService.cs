using RpcExtend.Collect;
using RpcExtend.Model.DB;
using RpcExtend.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.SysError;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Service
{
    internal class SysLogService : ISysLogService
    {
        private readonly ISysLogCollect _SysLog;
        private readonly IIocService _Unity;
        public SysLogService (ISysLogCollect sysLog, IIocService unity)
        {
            this._Unity = unity;
            this._SysLog = sysLog;
        }

        public void SaveSysLog (SysErrorLog[] logs, MsgSource source)
        {
            DateTime now = DateTime.Now;
            SystemErrorLog[] sys = logs.ConvertMap<SysErrorLog, SystemErrorLog>((a, b) =>
            {
                b.ServerId = source.ServerId;
                b.RpcMerId = source.RpcMerId;
                b.SystemType = source.SystemType;
                b.AddTime = now;
                if (b.SystemType == null)
                {
                    b.SystemType = string.Empty;
                }
                if (b.TraceId == null)
                {
                    b.TraceId = string.Empty;
                }
                if (b.LogShow == null)
                {
                    b.LogShow = string.Empty;
                }
                return b;
            });
            this._SysLog.AddLog(sys);
            logs.SendErrorLog(source, this._Unity);
        }
    }
}
