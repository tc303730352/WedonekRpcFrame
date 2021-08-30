using System;
using System.Collections.Concurrent;
using System.Linq;

namespace RpcHelper.TaskTools
{
        public class EntrustTaskCollect
        {
                private static readonly ConcurrentDictionary<Guid, IEntrustTask> _TaskList = new ConcurrentDictionary<Guid, IEntrustTask>();
                private static string _TaskId = null;
                private static readonly LockHelper _lock = new LockHelper();
                private static void _Init()
                {
                        if (_TaskId == null)
                        {
                                if (_lock.GetLock())
                                {
                                        if (_TaskId == null)
                                        {
                                                _TaskId = TaskManage.AddTask(new TaskHelper("委托任务!", new TimeSpan(0, 0, 1), _Sync));
                                        }
                                        _lock.Exit();
                                }
                        }
                }
                private static void _Sync()
                {
                        if (_TaskList.IsEmpty)
                        {
                                TaskManage.RemoveTask(_TaskId);
                                _TaskId = null;
                                return;
                        }
                        Guid[] ids = _TaskList.Keys.ToArray();
                        int time = HeartbeatTimeHelper.HeartbeatTime;
                        int ctime = time - 30;
                        ids.ForEach(a =>
                        {
                                if (_TaskList.TryGetValue(a, out IEntrustTask task))
                                {
                                        if (!task.IsEnd)
                                        {
                                                task.CheckIsOverTime(time);
                                        }
                                        else if (task.EndTime <= ctime)
                                        {
                                                _TaskList.TryRemove(a, out task);
                                        }
                                }
                        });
                }
                public static Guid CreateTask<Result>(EntrustFunc<Result> func, int timeout = 60)
                {
                        IEntrustTask task = new EntrustTask<Result>(func, timeout);
                        if (_TaskList.TryAdd(task.TaskId, task))
                        {
                                _Init();
                                task.ExecTask();
                                return task.TaskId;
                        }
                        return Guid.Empty;
                }
                public static Guid CreateTask<T, Result>(T param, EntrustFunc<T, Result> func, int timeout = 60)
                {
                        IEntrustTask task = new EntrustTask<T, Result>(param, func, timeout);
                        if (_TaskList.TryAdd(task.TaskId, task))
                        {
                                _Init();
                                task.ExecTask();
                                return task.TaskId;
                        }
                        return Guid.Empty;
                }
                public static bool GetTaskResult<Result>(Guid taskId, out TaskResult<Result> result, out string error)
                {
                        if (!_TaskList.TryGetValue(taskId, out IEntrustTask task))
                        {
                                result = null;
                                error = "entrust.task.not.find";
                                return false;
                        }
                        else
                        {
                                IEntrustTask<Result> res = (IEntrustTask<Result>)task;
                                result = res.GetResult();
                                error = null;
                                return true;
                        }
                }
        }
}
