using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using RpcModel.ErrorManage;

using RpcSyncService.Controller;
using RpcSyncService.Model;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcSyncService.Collect
{
        internal class ErrorCollect : IErrorEvent
        {
                public static readonly ErrorException DefError = new ErrorException("public.unknown");
                private static readonly ConcurrentDictionary<string, ErrorController> _ErrorList = new ConcurrentDictionary<string, ErrorController>();

                private static long _MaxErrorId = 0;
                static ErrorCollect()
                {
                        TaskManage.AddTask(new TaskHelper("保存错误文件!", new TimeSpan(0, 0, Tools.GetRandom(80, 150)), new Action(_SaveError)));
                        RpcClient.RpcClient.Route.AddRoute<RefreshError>("RefreshError", _RefreshError);
                }
                public static void Init()
                {
                        ErrorManage.SetAction(new ErrorCollect());
                        _MaxErrorId = ErrorManage.MaxErrorId;
                }
                private static void _RefreshError(RefreshError obj)
                {
                        if (ErrorManage.DropError(obj.ErrorId))
                        {
                                _SaveError();
                        }
                }
                public static void InitError(long errorId, string code)
                {
                        string key = string.Concat("Error_", errorId);
                        RpcClient.RpcClient.Cache.Remove(key);
                        key = string.Concat("Error_", code);
                        RpcClient.RpcClient.Cache.Remove(key);
                        if (ErrorManage.DropError(errorId))
                        {
                                _SaveError();
                        }
                        RpcClient.Collect.RemoteCollect.BroadcastMsg(new RefreshError
                        {
                                ErrorId = errorId
                        });
                }
                public bool DropError(long errorId)
                {
                        string code = this.FindErrorCode(errorId);
                        if (code == ErrorCollect.DefError.ErrorCode)
                        {
                                return false;
                        }
                        else if (_ErrorList.TryRemove(code, out ErrorController error))
                        {
                                error.Dispose();
                                return true;
                        }
                        return false;
                }

                private static void _SaveError()
                {
                        if (_ErrorList.IsEmpty)
                        {
                                return;
                        }
                        else if (Interlocked.Read(ref _MaxErrorId) != ErrorManage.MaxErrorId)
                        {
                                ErrorManage.SaveError();
                                _ErrorList.Clear();
                        }
                }

                public static void SetMaxErrorId(long errorId)
                {
                        if (Interlocked.Read(ref _MaxErrorId) < errorId)
                        {
                                Interlocked.Exchange(ref _MaxErrorId, errorId);
                        }
                }

                public string FindErrorCode(long errorId)
                {
                        string key = string.Concat("Error_", errorId);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out string code))
                        {
                                return code;
                        }
                        else if (!new DAL.ErrorCodeDAL().FindErrorCode(errorId, out code))
                        {
                                return null;
                        }
                        else if (string.IsNullOrEmpty(code))
                        {
                                RpcClient.RpcClient.Cache.Set(key, ErrorCollect.DefError.ErrorCode, new TimeSpan(0, 1, 0));
                                return ErrorCollect.DefError.ErrorCode;
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Set(key, code, new TimeSpan(30, 0, 0, 0));
                                return code;
                        }
                }



                public bool FindError(string code, string lang, out ErrorMsg msg)
                {
                        if (_GetError(code, out ErrorController obj))
                        {
                                msg = obj.GetError(lang);
                                return true;
                        }
                        msg = null;
                        return false;
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

                public ErrorMsg[] LoadError(string lang)
                {
                        if (_ErrorList.Count == 0)
                        {
                                return null;
                        }
                        ErrorController[] errors = _ErrorList.Values.ToArray();
                        return errors.ConvertAll(a => a.GetError(lang));
                }

                public static bool FindError(string code, out ErrorDatum datum, out string error)
                {
                        string key = string.Concat("Error_", code);
                        if (RpcClient.RpcClient.Cache.TryGet(key, out datum))
                        {
                                error = null;
                                return true;
                        }
                        else if (!new DAL.ErrorCodeDAL().SyncError(code, out datum))
                        {
                                error = "rpc.error.sync.error";
                                return false;
                        }
                        else
                        {
                                RpcClient.RpcClient.Cache.Set(key, datum, new TimeSpan(30, 0, 0, 0));
                                error = null;
                                return true;
                        }
                }

        }
}
