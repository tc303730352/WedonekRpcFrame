using System.Collections.Concurrent;
using RpcExtend.Collect;
using RpcExtend.Model.RetryTask;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Collect;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.ExtendModel;
using WeDonekRpc.ExtendModel.RetryTask.Model;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcExtend.Service.AutoRetry
{
    internal class AutoRetryTaskService
    {
        private static readonly ConcurrentDictionary<long, RetryTaskState> _RetryTask = new ConcurrentDictionary<long, RetryTaskState>();
        private static readonly ConcurrentDictionary<long, RetryTaskWheel> _TimerWheel = new ConcurrentDictionary<long, RetryTaskWheel>();

        private static Timer _RetryTaskTimer;

        private static Timer _ClearTaskTimer;

        private static IIocService _Ioc;
        public static void Init (IIocService ioc)
        {
            _Ioc = ioc;
        }
        public static void Start ()
        {
            if (_RetryTaskTimer == null)
            {
                _ = Task.Factory.StartNew(_LoadTask);
                _RetryTaskTimer = new Timer(_StartRetry, null, 0, 1000);
                _ClearTaskTimer = new Timer(_ClearTask, null, 10000, 10000);
            }
        }
        public static void Cancel (long taskId)
        {
            if (_RetryTask.TryGetValue(taskId, out RetryTaskState task))
            {
                task.Cancel();
            }
        }
        public static bool Add (RetryTask task)
        {
            RetryTaskState state = new RetryTaskState(task);
            if (!state.Init(DateTime.Now.ToLong()))
            {
                return false;
            }
            else if (_RetryTask.TryAdd(task.Id, state))
            {
                return _AddWheel(task.NextRetryTime, state);
            }
            return _RetryTask.ContainsKey(task.Id);
        }
        private static void _ClearTask (object state)
        {
            if (_TimerWheel.IsEmpty)
            {
                return;
            }
            long time = DateTime.Now.ToLong() - 5;
            long[] keys = _TimerWheel.Keys.Where(a => a <= time).ToArray();
            if (keys.Length == 0)
            {
                return;
            }
            keys.ForEach(c =>
            {
                if (_TimerWheel.TryRemove(c, out RetryTaskWheel wheel))
                {
                    wheel.Tasks.ForEach(a =>
                    {
                        if (_RetryTask.TryGetValue(a, out RetryTaskState state) && state.Status != AutoRetryTaskStatus.待重试)
                        {
                            _ = _RetryTask.TryRemove(a, out state);
                        }
                    });
                }
            });
        }
        private static void _LoadTask ()
        {
            using (IocScope scope = _Ioc.CreateScore())
            {
                IAutoRetryTaskCollect retryTask = scope.Resolve<IAutoRetryTaskCollect>();
                RetryTask[] tasks = retryTask.LoadRetryTask();
                if (tasks.IsNull())
                {
                    return;
                }
                long minTime = long.MaxValue;
                long now = DateTime.Now.ToLong();
                RetryTask[] results = tasks.Convert<RetryTask, RetryTask>(a =>
                {
                    RetryTaskState state = new RetryTaskState(a);
                    if (!state.Init(now))
                    {
                        return a;
                    }
                    if (_RetryTask.TryAdd(a.Id, state))
                    {
                        if (!_AddWheel(a.NextRetryTime, state))
                        {
                            return a;
                        }
                        else if (a.NextRetryTime < minTime)
                        {
                            minTime = a.NextRetryTime;
                        }
                    }
                    return null;
                });
                if (!results.IsNull())
                {
                    DateTime time = DateTime.Now;
                    retryTask.RetryResult(results.ConvertAll(a => new RetryTaskResult
                    {
                        ComplateTime = time,
                        ErrorCode = "rpc.retry.exec.expire",
                        Id = a.Id,
                        IsLock = false,
                        NextRetryTime = a.NextRetryTime,
                        Status = AutoRetryTaskStatus.已重试失败,
                        RetryNum = a.RetryNum
                    }));
                }
            }
        }
        private static bool _AddWheel (long time, RetryTaskState task)
        {
            RetryTaskWheel state = _TimerWheel.GetOrAdd(time, new RetryTaskWheel(time));
            if (!state.Add(task.Id))
            {
                time += 1;
                if (task.ResetTime(time))
                {
                    return _AddWheel(time, task);
                }
                return false;
            }
            return true;
        }
        private static void _StartRetry (object? obj)
        {
            long cur = DateTime.Now.ToLong();
            _ = Task.Factory.StartNew((time) =>
            {
                _StartTask((long)time);
            }, cur - 1);
            _StartTask(cur);
        }
        private static void _StartTask (long time)
        {
            if (!_TimerWheel.TryGetValue(time, out RetryTaskWheel state))
            {
                return;
            }
            long[] taskId = state.LockTask();
            if (taskId.Length == 0)
            {
                return;
            }
            using (IocScope scope = _Ioc.CreateScore())
            {
                IAutoRetryTaskCollect task = scope.Resolve<IAutoRetryTaskCollect>();
                task.LockTask(taskId);
                Dictionary<long, RetryTaskResult> result = taskId.ToDictionary(a => a, a => new RetryTaskResult { Id = a });
                DateTime now = time.ToDateTime();
                _ = Parallel.ForEach(taskId, a =>
                {
                    RetryTaskResult res = result[a];
                    RetryTaskState task = _RetryTask[res.Id];
                    res.RetryNum = task.Task.RetryNum;
                    if (_ExecTask(res, task, now))
                    {
                        if (!_AddWheel(res.NextRetryTime, task))
                        {
                            res.Status = AutoRetryTaskStatus.已重试失败;
                            res.ComplateTime = DateTime.Now;
                        }
                    }
                });
                task.RetryResult(result.Values.ToArray());
            }
        }
        private static bool _ExecTask (RetryTaskResult result, RetryTaskState task, DateTime time)
        {
            if (task.Status != AutoRetryTaskStatus.待重试)
            {
                result.Status = task.Status;
                return false;
            }
            _Send(task.Task, result, time);
            return task.SyncState(result);
        }
        private static void _Send (RetryTask task, RetryTaskResult result, DateTime time)
        {
            RpcParamConfig config = task.SendBody;
            IRemoteConfig send = new IRemoteConfig(config.SystemType)
            {
                SysDictate = config.SysDictate,
                RpcMerId = task.RpcMerId,
                RegionId = config.RegionId.GetValueOrDefault(task.RegionId),
                IsProhibitTrace = true
            };
            if (config.RemoteSet != null)
            {
                send = send.ConvertInto(config.RemoteSet);
                send.IsEnableLock = !send.LockColumn.IsNull();
            }
            DateTime end;
            if (!RemoteCollect.Send(send, config.MsgBody, out string error))
            {
                end = DateTime.Now;
                result.ErrorCode = error;
                result.Status = AutoRetryTaskStatus.已重试失败;
            }
            else
            {
                end = DateTime.Now;
                result.Status = AutoRetryTaskStatus.已重试成功;
                result.ComplateTime = end;
            }
            RetryLogSaveService.Add(task, result, ( end - time ).Milliseconds, time);
        }

        internal static void RunTask (RetryTask task)
        {
            RetryTaskResult result = new RetryTaskResult
            {
                Id = task.Id
            };
            _Send(task, result, DateTime.Now);
            if (result.Status == AutoRetryTaskStatus.已重试失败)
            {
                throw new ErrorException(result.ErrorCode);
            }
        }
    }
}
