using RpcClient.Interface;

using RpcHelper;

using RpcModel.SysError;

namespace RpcClient.Collect
{
        internal class SysLogCollect
        {
                internal static void Init()
                {
                        ErrorCollect.InitError();
                        LogSystem.AddErrorEvent(_SendError);
                        RpcClient.Config.AddRefreshEvent(_Refresh);
                }
                private static LogGrade _SendGradeLimit = LogGrade.ERROR;
                private static volatile bool _IsUpError = false;
                private static void _Refresh(IConfigServer config, string name)
                {
                        if (name.StartsWith("rpc_config") || name == string.Empty)
                        {
                                _IsUpError = Config.WebConfig.RpcConfig.IsUpLog;
                                _SendGradeLimit = Config.WebConfig.RpcConfig.LogGradeLimit;
                        }
                }
                private static void _SendLog(SysErrorLog[] errors)
                {
                        SaveErrorLog logs = new SaveErrorLog
                        {
                                Errors = errors
                        };
                        if (!RemoteCollect.Send(logs, out string error))
                        {
                                RpcLogSystem.AddLocalErrorLog("SaveErrorLog", logs, "Basic_Server", error);
                        }
                }
                private static void _SendError(LogInfo[] log)
                {
                        if (!RpcStateCollect.IsInit || !_IsUpError)
                        {
                                return;
                        };
                        log = log.FindAll(a => a.IsLocal == false && a.LogGrade >= _SendGradeLimit);
                        if (log.Length == 0)
                        {
                                return;
                        }
                        _SendLog(log.ConvertAll(a =>
                        {
                                return new SysErrorLog
                                {
                                        AddTime = a.AddTime,
                                        Content = a.LogContent,
                                        LogType = a.LogType,
                                        LogGrade = a.LogGrade,
                                        Title = a.LogTitle,
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
