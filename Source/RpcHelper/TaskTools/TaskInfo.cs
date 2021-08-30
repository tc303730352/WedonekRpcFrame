using System;

namespace RpcHelper.TaskTools
{
        internal class TaskInfo : ITask
        {
                public TaskInfo(ITask task)
                {
                        task.TaskId = Guid.NewGuid().ToString("N");
                        this._Task = task;
                        DateTime now = DateTime.Now;
                        if (task.TaskType == TaskType.间隔任务)
                        {
                                this._NextExecTime = now.AddSeconds(task.ExecInterval.Seconds);
                        }
                        else if (task.TaskType == TaskType.定时间隔任务)
                        {
                                DateTime time = now.Date.Add(task.ExecInterval);
                                if (time <= now)
                                {
                                        double sec = (now - time).TotalSeconds;//7-2=5
                                        int num = (int)task.ExecInterval.TotalSeconds;//2
                                        int size = (int)Math.Ceiling(sec / num);
                                        num *= size;
                                        this._NextExecTime = time.AddSeconds(num);
                                }
                                else
                                {
                                        this._NextExecTime = time;
                                }
                        }
                        else
                        {

                                DateTime time = now.Date.Add(task.ExecInterval);
                                this._NextExecTime = time < now ? time.AddDays(1) : time;
                        }
                }
                public TaskType TaskType => this._Task.TaskType;
                public bool IsOnce => this._Task.IsOnce;
                /// <summary>
                /// 任务
                /// </summary>
                private readonly ITask _Task = null;
                /// <summary>
                /// 下次执行时间
                /// </summary>
                private DateTime _NextExecTime = DateTime.MaxValue;

                public TimeSpan ExecInterval => this._Task.ExecInterval;
                public string TaskId
                {
                        set => this._Task.TaskId = value;

                        get => this._Task.TaskId;
                }

                public string TaskName => this._Task.TaskName;

                public int TaskPriority => this._Task.TaskPriority;

                private bool _ExecuteTask()
                {
                        try
                        {
                                this._Task.ExecuteTask();
                                return true;
                        }
                        catch (Exception e)
                        {
                                new ErrorLog(ErrorException.FormatError(e), "任务执行失败", "LocalTask") { LogContent = this.TaskName }.Save();
                                return false;
                        }
                }
                public void ExecuteTask()
                {
                        DateTime now = DateTime.Now;
                        if (this._NextExecTime <= now)
                        {
                                this._NextExecTime = this.TaskType == TaskType.定时任务
                                        ? this._NextExecTime.AddDays(1)
                                        : this.TaskType == TaskType.定时间隔任务 ? this._NextExecTime.Add(this.ExecInterval) : now.Add(this.ExecInterval);
                                this._ExecuteTask();
                        }
                }

                public void Dispose()
                {
                        this._Task.Dispose();
                }
        }
}
