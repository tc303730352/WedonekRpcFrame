using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using RpcClient.Controller;
using RpcClient.Interface;
using RpcClient.Resource;

using RpcModel.ErrorManage;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcClient.Collect
{
        [Attr.ClassLifetimeAttr(Attr.ClassLifetimeType.单例)]
        internal class ErrorCollect : IErrorEvent, IErrorCollect
        {
                private static readonly ConcurrentDictionary<string, ErrorController> _ErrorList = new ConcurrentDictionary<string, ErrorController>();
                private static readonly ConcurrentDictionary<long, string> _ErrorCode = new ConcurrentDictionary<long, string>();

                private static long _NewErrorId = 0;

                private static void _Refresh(RefreshError obj)
                {
                        if (ErrorManage.DropError(obj.ErrorId))
                        {
                                ErrorManage.SaveError();
                        }
                }

                public static void InitError()
                {
                        if (Config.WebConfig.IsEnableError)
                        {
                                _NewErrorId = ErrorManage.MaxErrorId;
                                ErrorManage.SetAction(new ErrorCollect());
                                RpcClient.Route.AddRoute<RefreshError>("RefreshError", _Refresh);
                                TaskManage.AddTask(new TaskHelper("刷新错误信息!", new System.TimeSpan(0, 0, Tools.GetRandom(60, 90)), new Action(_ClearError)));
                        }
                        else
                        {
                                RpcLogSystem.AddLog("错误集中管理未开启!");
                        }
                }

                private static void _ClearError()
                {
                        if (_ErrorList.Count == 0)
                        {
                                return;
                        }
                        else if (Interlocked.Read(ref _NewErrorId) != ErrorManage.MaxErrorId)
                        {
                                ErrorManage.SaveError();
                                _ErrorList.Clear();
                                _ErrorCode.Clear();
                                _NewErrorId = ErrorManage.MaxErrorId;
                        }
                }


                public static void SetError(long errorId, string code)
                {
                        if (_ErrorCode.ContainsKey(errorId))
                        {
                                return;
                        }
                        else if (_ErrorCode.TryAdd(errorId, code))
                        {
                                Interlocked.Exchange(ref _NewErrorId, errorId);
                        }
                }
                public void ResetError(long errorId, string code)
                {
                        InitError obj = new InitError
                        {
                                ErrorCode = code,
                                ErrorId = errorId
                        };
                        if (!RemoteCollect.Send(obj, out string error))
                        {
                                throw new ErrorException(error);
                        }
                }
                private static bool _GetError(string code, out ErrorController error)
                {
                        if (!_ErrorList.TryGetValue(code, out error))
                        {
                                error = _ErrorList.GetOrAdd(code, new ErrorController(code));
                        }
                        if (!error.Init())
                        {
                                _ErrorList.TryRemove(code, out error);
                                error.Dispose();
                                return false;
                        }
                        return error.IsInit;
                }

                public string FindErrorCode(long errorId)
                {
                        return _ErrorCode.TryGetValue(errorId, out string code) ? code : ErrorRemote.FindError(errorId, out code, out _) ? code : null;
                }

                public bool FindError(string code, string lang, out ErrorMsg msg)
                {
                        if (_GetError(code, out ErrorController obj))
                        {
                                msg = obj.GetErrorMsg(lang);
                                return true;
                        }
                        else
                        {
                                msg = null;
                                return false;
                        }
                }

                public ErrorMsg[] LoadError(string lang)
                {
                        if (_ErrorList.IsEmpty)
                        {
                                return null;
                        }
                        ErrorController[] vals = _ErrorList.Values.ToArray();
                        return vals.ConvertAll(a => a.GetErrorMsg(lang));
                }

                public bool DropError(long errorId)
                {
                        if (_ErrorCode.TryRemove(errorId, out string code))
                        {
                                if (_ErrorList.TryRemove(code, out ErrorController error))
                                {
                                        error.Dispose();
                                }
                                return true;
                        }
                        return false;
                }
        }
}
