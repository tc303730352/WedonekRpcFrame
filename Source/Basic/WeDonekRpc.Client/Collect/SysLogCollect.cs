using System.Linq;
using WeDonekRpc.Client.Config;
using WeDonekRpc.Client.Log;
using WeDonekRpc.ExtendModel.SysError;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Config;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Client.Collect
{
    /// <summary>
    /// 系统日志发送模块
    /// </summary>
    internal class SysLogCollect
    {
        internal static void Init ()
        {
            RpcClient.Config.GetSection("rpc:Log").AddRefreshEvent(_Refresh);
            //向本地日志系统注册事件
            LogSystem.AddErrorEvent(_FiltersLog);
        }
        /// <summary>
        /// 发送的日志信息级别
        /// </summary>
        private static LogGrade _SendGradeLimit = LogGrade.ERROR;
        private static string[] _Excludes = null;
        /// <summary>
        /// 是否上传日志信息
        /// </summary>
        private static volatile bool _IsUpLog = false;

        private static void _Refresh (IConfigSection section, string name)
        {
            LogConfig config = section.GetValue(new LogConfig());
            _IsUpLog = config.IsUpLog;
            _SendGradeLimit = config.LogGradeLimit;
            _Excludes = config.ExcludeLogGroup;
        }
        /// <summary>
        /// 发送错误日志
        /// </summary>
        /// <param name="errors"></param>
        private static void _SendLog (SysErrorLog[] errors)
        {
            SaveErrorLog logs = new SaveErrorLog
            {
                Errors = errors
            };
            if (!RemoteCollect.Send(logs, out string error))
            {
                RpcLogSystem.AddLocalErrorLog("SaveErrorLog", "错误消息发送失败", logs, "Basic_Server", error);
            }
        }
        /// <summary>
        /// 日志过滤
        /// </summary>
        /// <param name="log"></param>
        private static void _FiltersLog (LogInfo[] log)
        {
            if (!RpcStateCollect.IsInit || !_IsUpLog)
            {
                return;
            }
            else if (_Excludes.IsNull())
            {
                log = log.FindAll(a => a.IsLocal == false && a.LogGrade >= _SendGradeLimit);
            }
            else
            {
                log = log.FindAll(a => a.IsLocal == false && !_Excludes.Contains(a.Type) && a.LogGrade >= _SendGradeLimit);
            }
            if (log.Length == 0)
            {
                return;
            }
            _SendLog(log.ConvertAll(a =>
            {
                return new SysErrorLog
                {
                    AddTime = a.AddTime,
                    LogShow = a.LogContent,
                    LogType = a.LogType,
                    LogGrade = a.LogGrade,
                    LogTitle = a.LogTitle,
                    TraceId = a["TraceId"],
                    ErrorCode = a.ErrorCode,
                    LogGroup = a.Type,
                    AttrList = a.ToJson(),
                    Exception = a.Exception != null ? new ExceptionMsg(a.Exception) : null
                };
            }));
        }
    }
}
