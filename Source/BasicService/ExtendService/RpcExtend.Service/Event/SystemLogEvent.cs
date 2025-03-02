using RpcExtend.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.SysError;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Event
{
    /// <summary>
    /// 系统日志模块
    /// </summary>
    internal class SystemLogEvent : IRpcApiService
    {
        private readonly ISysLogService _SysLog;
        public SystemLogEvent (ISysLogService sysLog)
        {
            this._SysLog = sysLog;
        }

        /// <summary>
        /// 保存错误日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="source"></param>
        public void SaveErrorLog (SaveErrorLog log, MsgSource source)
        {
            this._SysLog.SaveSysLog(log.Errors, source);
        }
    }
}
