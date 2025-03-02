using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace WeDonekRpc.Helper.TaskTools
{
    public class EntrustTaskCollect
    {
        private static readonly ConcurrentDictionary<Guid, IEntrustTask> _TaskList = new ConcurrentDictionary<Guid, IEntrustTask>();
        private static readonly Timer _SyncTimer = null;
        static EntrustTaskCollect ()
        {
            if (_SyncTimer == null)
            {
                _SyncTimer = new Timer(_Sync, null, 1000, 1000);
            }
        }
        private static void _Sync (object state)
        {
            if (_TaskList.IsEmpty)
            {
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
                        _ = _TaskList.TryRemove(a, out task);
                    }
                }
            });
        }
        public static Guid CreateTask<Result> (EntrustFunc<Result> func, int timeout = 60)
        {
            IEntrustTask task = new EntrustTask<Result>(func, timeout);
            if (_TaskList.TryAdd(task.TaskId, task))
            {
                task.ExecTask();
                return task.TaskId;
            }
            return Guid.Empty;
        }
        public static Guid CreateTask<Result> (EntrustFunc<Result> func, Action end, int timeout = 60)
        {
            IEntrustTask task = new EntrustTask<Result>(func, end, timeout);
            if (_TaskList.TryAdd(task.TaskId, task))
            {
                task.ExecTask();
                return task.TaskId;
            }
            return Guid.Empty;
        }
        public static Guid CreateTask<T, Result> (T param, EntrustFunc<T, Result> func, int timeout = 60)
        {
            IEntrustTask task = new EntrustTask<T, Result>(param, func, timeout);
            if (_TaskList.TryAdd(task.TaskId, task))
            {
                task.ExecTask();
                return task.TaskId;
            }
            return Guid.Empty;
        }
        public static Guid CreateTask<T, Result> (T param, EntrustFunc<T, Result> func, Action end, int timeout = 60)
        {
            IEntrustTask task = new EntrustTask<T, Result>(param, func, end, timeout);
            if (_TaskList.TryAdd(task.TaskId, task))
            {
                task.ExecTask();
                return task.TaskId;
            }
            return Guid.Empty;
        }
        public static TaskResult<Result> GetTaskResult<Result> (Guid taskId)
        {
            if (!_TaskList.TryGetValue(taskId, out IEntrustTask task))
            {
                throw new ErrorException("entrust.task.not.find");
            }
            else
            {
                IEntrustTask<Result> res = (IEntrustTask<Result>)task;
                return res.GetResult();
            }
        }
    }
}
