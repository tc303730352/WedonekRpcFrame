using System.Collections.Generic;
using System.Linq;
using System.Threading;

using AutoTaskService.Controller;
using AutoTaskService.Model;

using RpcHelper;
namespace AutoTaskService.Collect
{
        internal class AutoTaskCollect
        {
                private static readonly Dictionary<long, TaskController> _TaskList = new Dictionary<long, TaskController>();
                private static readonly Timer _Timer = new Timer(new TimerCallback(a => LoadTask()), null, 60000, 6000);

                public static void LoadTask()
                {
                        if (!new DAL.AutoTaskDAL().GetAllTask(out BasicTask[] tasks))
                        {
                                new WarnLog("rpc.task.load.error", "任务加载错误!");
                                return;
                        }
                        _ClearTask(tasks);
                        tasks.ForEach(a =>
                        {
                                if (!_TaskList.TryGetValue(a.Id, out TaskController task))
                                {
                                        _LoadTask(a.Id);
                                }
                                else if (task.VerNum != a.VerNum)
                                {
                                        _ResetTask(task);
                                }
                        });
                }
                private static void _ResetTask(TaskController task)
                {
                        if (!task.Reset(out string error))
                        {
                                _TaskList.Remove(task.Id);
                                return;
                        }
                }
                private static void _LoadTask(long id)
                {
                        TaskController task = new TaskController(id);
                        if (!task.Init(out string error))
                        {
                                task.Dispose();
                                return;
                        }
                        _TaskList.Add(id, task);
                }
                private static void _ClearTask(BasicTask[] tasks)
                {
                        if (_TaskList.Count == 0)
                        {
                                return;
                        }
                        long[] removeId = _TaskList.Keys.ToArray().Remove(tasks, (a, b) => a == b.Id);
                        if (removeId.Length > 0)
                        {
                                removeId.ForEach(a =>
                                {
                                        _TaskList[a].Dispose();
                                        _TaskList.Remove(a);
                                });
                        }
                }
        }
}
