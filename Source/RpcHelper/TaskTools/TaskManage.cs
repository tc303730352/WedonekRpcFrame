using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RpcHelper.TaskTools
{
        public class TaskManage
        {
                private static readonly HashSet<ITask> _TaskList = new HashSet<ITask>();

                private static readonly Thread _ExecThread = new Thread(new ThreadStart(_ExecTask));

                private static int _MinSleepTime = 1000;

                private static bool _IsStop = false;

                private static void _ExecTask()
                {
                        do
                        {
                                Thread.Sleep(_MinSleepTime);
                                if (_TaskList.Count > 0)
                                {
                                        ITask[] list = null;
                                        if (_Lock.GetReadLock())
                                        {
                                                list = _TaskList.ToArray();
                                                _Lock.ExitRead();
                                        }
                                        if (list != null && list.Length > 0)
                                        {
                                                int[] groupId = list.Select(a => a.TaskPriority).OrderByDescending(a => a).Distinct().ToArray();
                                                if (groupId.Length == 1)
                                                {
                                                        _ExecTask(list);
                                                }
                                                else
                                                {
                                                        groupId.ForEach(a =>
                                                        {
                                                                ITask[] tasks = list.FindAll(b => b.TaskPriority == a);
                                                                _ExecTask(tasks);
                                                        });
                                                }
                                                _MinSleepTime = (int)list.Min(a => a.ExecInterval.TotalMilliseconds);
                                                if (_MinSleepTime <= 50)
                                                {
                                                        _MinSleepTime = 50;
                                                }
                                                else if (_MinSleepTime > 30000)
                                                {
                                                        _MinSleepTime = 30000;
                                                }
                                        }
                                }
                        } while (!_IsStop);
                }
                private static void _ExecTask(ITask[] tasks)
                {
                        if (tasks.Length == 1)
                        {
                                _BeginTask(tasks[0]);
                        }
                        else
                        {
                                Parallel.ForEach(tasks, _BeginTask);
                        }
                }
                private static void _BeginTask(ITask task)
                {
                        try
                        {
                                task.ExecuteTask();
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(ErrorException.FormatError(e), "任务执行错误!", "LocalTask") { LogContent = task.TaskName }.Save();
                        }
                        finally
                        {
                                if (task.IsOnce)
                                {
                                        if (_Lock.GetWriteLock())
                                        {
                                                _TaskList.Remove(task);
                                                _Lock.ExitWrite();
                                        }
                                }
                        }
                }
                static TaskManage()
                {
                        if (_ExecThread.ThreadState == System.Threading.ThreadState.Unstarted)
                        {
                                _ExecThread.IsBackground = true;
                                _ExecThread.Start();
                        }
                        Process pro = Process.GetCurrentProcess();
                        pro.EnableRaisingEvents = true;
                        pro.Exited += new EventHandler(pro_Exited);
                }

                private static void pro_Exited(object sender, EventArgs e)
                {
                        StopTask();
                }

                private static readonly ReadWriteLockHelper _Lock = new ReadWriteLockHelper();

                private static string _AddTask(ITask task)
                {
                        if (_Lock.GetWriteLock())
                        {
                                _TaskList.Add(task);
                                _Lock.ExitWrite();
                        }
                        if (task.ExecInterval.TotalMilliseconds < _MinSleepTime)
                        {
                                if (_ExecThread.ThreadState == System.Threading.ThreadState.WaitSleepJoin)
                                {
                                        _ExecThread.Resume();
                                }
                        }
                        return task.TaskId;
                }
                private static void _AddTask(ITask[] task)
                {
                        if (_Lock.GetWriteLock())
                        {
                                Array.ForEach(task, a => _TaskList.Add(a));
                                _Lock.ExitWrite();
                        }
                }

                public static void StopTask()
                {
                        ITask[] tasks = _StopTask();
                        if (tasks.Length > 0)
                        {
                                Parallel.ForEach(tasks, a =>
                                 {
                                         a.Dispose();
                                 });
                        }
                }
                private static ITask[] _StopTask()
                {
                        _IsStop = true;
                        ITask[] tasks = null;
                        if (_Lock.GetWriteLock())
                        {
                                tasks = _TaskList.ToArray();
                                _TaskList.Clear();
                                _Lock.ExitWrite();
                        }
                        return tasks;
                }
                public static void RemoveTask(string taskId)
                {
                        try
                        {
                                if (_Lock.GetWriteLock())
                                {
                                        _TaskList.RemoveWhere(a => a.TaskId == taskId);
                                        _Lock.ExitWrite();
                                }
                        }
                        catch (Exception)
                        {

                        }
                }
                public static string AddTask(ITask task)
                {
                        return _AddTask(new TaskInfo(task));
                }
                public static void AddTask(ITask[] task)
                {
                        ITask[] tasks = Array.ConvertAll<ITask, ITask>(task, a => new TaskInfo(a));
                        _AddTask(tasks);
                }
        }
}
