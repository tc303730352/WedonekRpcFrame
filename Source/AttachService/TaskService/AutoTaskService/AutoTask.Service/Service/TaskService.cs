using System.Collections.Concurrent;
using AutoTask.Collect;
using AutoTask.Model.Task;
using AutoTask.Service.AutoTask;
using RpcTaskModel.AutoTask.Msg;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace AutoTask.Service.Service
{
    internal class TaskService
    {
        private static readonly ConcurrentDictionary<long, SingleTask> _TaskList = new ConcurrentDictionary<long, SingleTask>();
        private static long[] _Keys;
        private static Timer _Timer;
        private static Timer _ExecTimer;

        static TaskService ()
        {
            RpcClient.Route.AddRoute<EndTaskEvent>("EndTask", _EndTask);
        }
        private static void _EndTask ( EndTaskEvent obj )
        {
            if ( _TaskList.TryGetValue(obj.TaskId, out SingleTask task) )
            {
                if ( obj.IsEnd )
                {
                    task.EndTask();
                }
                else
                {
                    task.StopTask();
                }
            }
        }

        private static void _Load ( object state )
        {
            using ( IocScope scope = RpcClient.Ioc.CreateScore() )
            {
                IAutoTaskCollect autoTask = scope.Resolve<IAutoTaskCollect>();
                MsgSource source = RpcClient.CurrentSource;
                BasicTask[] tasks = autoTask.GetTasks(source.RpcMerId);
                if ( !tasks.IsNull() )
                {
                    tasks = tasks.OrderBy(a => a.TaskPriority).ToArray();
                    tasks.ForEach(a =>
                    {
                        if ( !_TaskList.TryGetValue(a.Id, out SingleTask task) )
                        {
                            _LoadTask(a);
                        }
                        else if ( task.VerNum != a.VerNum )
                        {
                            task.Reset(a.VerNum);
                        }
                    });
                }
                if ( !_Keys.IsNull() )
                {
                    _Keys.ForEach(c => !tasks.IsExists(a => a.Id == c), c =>
                    {
                        if ( _TaskList.TryGetValue(c, out SingleTask task) )
                        {
                            task.StopTask();
                        }
                    });
                }
                _Keys = _TaskList.Keys.ToArray();
            }
        }

        public static void Init ()
        {
            _Load(null);
            _Timer = new Timer(new TimerCallback(_Load), null, 30000, 30000);
            _ExecTimer = new Timer(new TimerCallback(_ExecTask), null, 1000, 1000);
        }
        private static void _ExecTask ( object state )
        {
            if ( _Keys.IsNull() )
            {
                return;
            }
            int time = HeartbeatTimeHelper.HeartbeatTime;
            _Keys.ForEachByParallel(a =>
            {
                using ( IocScope scope = RpcClient.Ioc.CreateScore() )
                {
                    if ( _TaskList.TryGetValue(a, out SingleTask task) )
                    {
                        if ( task.ExecTask(time) )
                        {
                            _ = _TaskList.TryRemove(a, out task);
                        }
                    }
                }
            });
        }
        private static void _LoadTask ( BasicTask obj )
        {
            SingleTask task = new SingleTask(obj.Id, obj.VerNum);
            if ( task.Init() )
            {
                _ = _TaskList.TryAdd(obj.Id, task);
            }
        }
        public static void Close ()
        {
            _Timer.Dispose();
            _ExecTimer?.Dispose();
        }
    }
}
