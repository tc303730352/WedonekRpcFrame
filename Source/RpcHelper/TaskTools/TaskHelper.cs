using System;

namespace RpcHelper.TaskTools
{
        /// <summary>
        /// 单个任务
        /// </summary>
        public class TaskHelper : ITask
        {
                /// <summary>
                /// 单个任务(默认间隔指定时间执行)
                /// </summary>
                /// <param name="name">任务名</param>
                /// <param name="action">委托执行的方法</param>
                /// <param name="stopEvent">任务停止时调用的委托</param>
                /// <param name="priority">优先级</param>
                public TaskHelper(string name, Action action, Action stopEvent, int priority = 1) : this(name, action, priority)
                {
                        this._StopAction = stopEvent;
                }
                /// <summary>
                /// 单个任务(默认间隔指定时间执行)
                /// </summary>
                /// <param name="name">任务名</param>
                /// <param name="time">任务间隔时间(最小单位：秒)</param>
                /// <param name="action">委托执行的方法</param>
                /// <param name="stopEvent">任务停止时调用的委托</param>
                /// <param name="priority">优先级</param>
                public TaskHelper(string name, TimeSpan time, Action action, Action stopEvent, int priority = 1) : this(name, time, action, priority)
                {
                        this._StopAction = stopEvent;
                }
                /// <summary>
                /// 单个任务(在一分到两分之间随机一个间隔时间执行)
                /// </summary>
                /// <param name="name">任务名</param>
                /// <param name="fun">委托执行的方法</param>
                public TaskHelper(string name, Action fun, int priority = 1) : this(name, TaskType.间隔任务, new TimeSpan(0, 0, Tools.GetRandom(60, 120)), fun, priority)
                {
                }
                /// <summary>
                /// 单个任务(默认间隔指定时间执行)
                /// </summary>
                /// <param name="name">任务名</param>
                /// <param name="time">任务间隔时间(最小单位：秒)</param>
                /// <param name="fun">委托执行的方法</param>
                /// <param name="priority">优先级</param>
                public TaskHelper(string name, TimeSpan time, Action fun, int priority = 1) : this(name, TaskType.间隔任务, time, fun, priority)
                {
                }

                public TaskHelper(string name, TaskType taskType, TimeSpan time, Action fun, int priority = 1)
                {
                        this._TaskName = name;
                        this._Interval = time;
                        this._TaskType = taskType;
                        this._TaskFun = fun;
                        this._SetTaskPriority(priority);
                }

                public TaskHelper(string taskName, TaskType taskType, TimeSpan timeSpan, Action action, string id, int taskPriority) : this(taskName, taskType, timeSpan, action, taskPriority)
                {
                        this.TaskId = id;
                }

                private void _SetTaskPriority(int priority)
                {
                        if (priority == 0)
                        {
                                this._TaskPriority = Tools.GetRandom();
                        }
                        else
                        {
                                this._TaskPriority = priority;
                        }
                }
                private readonly Action _StopAction = null;
                private readonly Action _TaskFun = null;
                public string TaskId { get; set; }

                private readonly string _TaskName = null;

                public string TaskName => this._TaskName;
                private readonly TaskType _TaskType = TaskType.间隔任务;


                public TaskType TaskType => this._TaskType;

                private readonly TimeSpan _Interval = new TimeSpan(0, 1, 0);
                public TimeSpan ExecInterval => this._Interval;

                public bool IsOnce => false;

                private int _TaskPriority = 0;


                public int TaskPriority => this._TaskPriority;

                public void ExecuteTask()
                {
                        this.ExecTask();
                }
                protected virtual void ExecTask()
                {
                        if (this._TaskFun != null)
                        {
                                this._TaskFun.Invoke();
                        }
                }

                public void Dispose()
                {
                        this._StopAction?.Invoke();
                }
        }
}
