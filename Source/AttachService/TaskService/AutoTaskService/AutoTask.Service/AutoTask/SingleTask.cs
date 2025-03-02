using AutoTask.Collect;
using AutoTask.Collect.Model;
using AutoTask.Model;
using AutoTask.Model.Task;
using AutoTask.Model.TaskItem;
using AutoTask.Service.DelaySave;
using AutoTask.Service.Helper;
using AutoTask.Service.Model;
using RpcTaskModel;
using RpcTaskModel.TaskItem.Model;
using RpcTaskModel.TaskPlan.Model;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Http;
using WeDonekRpc.Helper.Interface;

namespace AutoTask.Service.AutoTask
{
    /// <summary>
    /// 任务控制器
    /// </summary>
    internal class SingleTask
    {
        /// <summary>
        /// 执行状态 0 = 待初始化 ,1 =待执行 ,2 = 任务已结束, 3, 终止任务  4 =执行中
        /// </summary>
        private int _ExecStatus = 0;
        public SingleTask (long id, int verNum)
        {
            this.Id = id;
            this.VerNum = verNum;
        }
        private IAutoTaskCollect _TaskCollect
        {
            get => RpcClient.Ioc.Resolve<IAutoTaskCollect>();
        }
        private ITaskItemCollect _TaskItemCollect
        {
            get => RpcClient.Ioc.Resolve<ITaskItemCollect>();
        }
        private ITaskPlanCollect _TaskPlanCollect
        {
            get => RpcClient.Ioc.Resolve<ITaskPlanCollect>();
        }

        public long Id { get; }

        private RemoteTask _Task;

        private TaskPlanBasic[] _Plans;

        private TaskItemDto[] _Items;

        private volatile int _ExecTime = 0;

        private DateTime _NextExecTime;

        private volatile int _ExecVerNum = 0;

        private int _TaskNum = 0;

        public int VerNum
        {
            get;
            private set;
        }

        public int TaskPriority
        {
            get;
            private set;
        }
        public bool Init ()
        {
            if (this._InitTask(true))
            {
                _ = Interlocked.Exchange(ref this._ExecStatus, 1);//启用
                return true;
            }
            return false;
        }
        private bool _InitState (bool isRefresh = false)
        {
            TaskState state = this._TaskCollect.GetTaskState(this.Id, isRefresh);
            if (state.TaskStatus == AutoTaskStatus.启用)
            {
                if (state.NextExecTime.HasValue && this._Plans != null)
                {
                    this._NextExecTime = this._Plans.GetExecTime(state.NextExecTime.Value);
                    this._ExecTime = HeartbeatTimeHelper.GetTime(this._NextExecTime);
                }
                else
                {
                    this._ExecTime = 0;
                }
                this._ExecVerNum = state.ExecVerNum;
                return true;
            }
            return false;
        }
        private bool _InitTask (bool isRefresh)
        {
            if (!this._InitState(isRefresh))
            {
                return false;
            }
            this._Init();
            if (this._ExecTime == 0)
            {
                DateTime time = this._Plans.GetExecTime(this._NextExecTime);
                this._TaskCollect.SetTaskTime(this.Id, time);
                this._NextExecTime = time;
                this._ExecTime = HeartbeatTimeHelper.GetTime(time);
            }
            return true;
        }
        private void _Init ()
        {
            this._Task = this._TaskCollect.GetTask(this.Id, this.VerNum);
            this._Plans = this._TaskPlanCollect.GetTaskPlans(this.Id, this.VerNum);
            AutoTaskItem[] items = this._TaskItemCollect.Gets(this.Id, this.VerNum);
            this._Items = items.OrderBy(a => a.ItemSort).ToArray().ConvertMap<AutoTaskItem, TaskItemDto>();
            this.TaskPriority = this._Task.TaskPriority;
            this._RegHttpClient(this._Items);
        }
        private void _RegHttpClient ( TaskItemDto[] items)
        {
            TaskItemDto[] https = items.FindAll(a => a.SendType == TaskSendType.Http);
            if ( https.IsNull() )
            {
                return;
            } 
            https.ForEach(c =>
            {
                c.ClientKey = "TaskItem_" + c.Id;
                HttpConfig config = c.SendParam.HttpConfig.Config;
                if ( config == null )
                {
                    HttpClientFactory.SetClient(c.ClientKey);
                }
                else
                {
                    HttpClientFactory.SetClient(c.ClientKey, a =>
                    {
                        a.AllowAutoRedirect=config.AllowAutoRedirect;
                        a.AutomaticDecompression=config.AutomaticDecompression;
                        a.MaxAutomaticRedirections = config.MaxAutomaticRedirections;
                        a.SslProtocols=config.SslProtocols;
                    });
                }
            });
        }

        public bool ExecTask (int time)
        {
            if (this._ExecTime == 0 || this._ExecTime > time)
            {
                return this._IsRemove();
            }
            else if (Interlocked.CompareExchange(ref this._ExecStatus, 4, 1) == 1)
            {
                DateTime next = this._Plans.GetNextTime(DateTime.Now);
                this._ExecTime = HeartbeatTimeHelper.GetTime(next);
                if (this._TaskCollect.BeginExec(this.Id, next, this._ExecVerNum, out int ver))
                {
                    this._ExecVerNum = ver;
                    this._BeginExec(this._Task.BeginStep);
                    return false;
                }
                else if (!this._InitState(true))
                {
                    this.StopTask();
                    return true;
                }
                return false;
            }
            return this._IsRemove();
        }
        private void _EndTask ()
        {
            _ = Interlocked.CompareExchange(ref this._ExecStatus, 1, 4);
            this._TaskCollect.ExecEnd(this.Id);
        }

        private void _BeginExec (int index)
        {
            TaskItemDto[] items = this._Items.FindAll(c => c.ItemSort == index);
            if (items.Length > 0)
            {
                _ = Interlocked.Exchange(ref this._TaskNum, items.Length);
                items.ForEach(a => this._ExecItem(a));
            }
            else
            {
                this._EndTask();
            }
        }
        private void _RootEnd ()
        {
            int num = Interlocked.Add(ref this._TaskNum, -1);
            if (num == 0)
            {
                this._EndTask();
            }
        }
        private void _Exec (int index)
        {
            TaskItemDto[] items = this._Items.FindAll(c => c.ItemSort == index);
            if (items.Length == 1)
            {
                this._ExecItem(items[0]);
            }
            else if (items.Length > 0)
            {
                _ = Interlocked.Add(ref this._TaskNum, items.Length - 1);
                items.ForEach(a => this._ExecItem(a));
            }
            else
            {
                this._RootEnd();
            }
        }
        private void _TaskFail ( TaskItemDto item )
        {
            if (Interlocked.CompareExchange(ref this._ExecStatus, 2, 3) == 3)
            {
                this._TaskCollect.ExecEnd(this.Id);
            }
            else if (item.FailStep == TaskStep.停止执行)
            {
                this._RootEnd();
            }
            else if (item.FailStep == TaskStep.继续下一步)
            {
                this._Exec(item.ItemSort + 1);
            }
            else
            {
                this._Exec(item.FailNextStep.Value);
            }
        }
        private void _TaskSuccess (TaskItemDto item)
        {
            if (Interlocked.CompareExchange(ref this._ExecStatus, 2, 3) == 3)
            {
                this._TaskCollect.ExecEnd(this.Id);
            }
            else if (item.SuccessStep == TaskStep.停止执行)
            {
                this._RootEnd();
            }
            else if (item.SuccessStep == TaskStep.继续下一步)
            {
                this._Exec(item.ItemSort + 1);
            }
            else
            {
                this._Exec(item.NextStep.Value);
            }
        }
        private async void _ExecItem ( TaskItemDto item )
        {
            await Task.Run(() =>
            {
                using (IocScope scope = RpcClient.Ioc.CreateScore())
                {
                    TaskExecLog[] logs = item.ExecTask(this._Task.RpcMerId, this._Task.RegionId);
                    TaskLogService.Adds(logs, item, this.Id);
                    TaskExecLog[] fail = logs.FindAll(c => c.isFail);
                    bool isSuccess = true;
                    string error = null;
                    if (fail.Length != 0)
                    {
                        isSuccess = false;
                        error = fail[fail.Length - 1].error;
                    }
                    TaskItemStatusService.ExecComplate(new SyncItemResult
                    {
                        ItemId = item.Id,
                        IsSuccess = isSuccess,
                        Error = error
                    });
                    if (isSuccess)
                    {
                        this._TaskSuccess(item);
                    }
                    else
                    {
                        this._TaskFail(item);
                    }
                }
            });
        }
        /// <summary>
        /// 停止任务
        /// </summary>
        public void StopTask ()
        {
            _ = Interlocked.Exchange(ref this._ExecStatus, 2);
        }
        private bool _IsRemove ()
        {
            return Interlocked.CompareExchange(ref this._ExecStatus, 0, 0) == 2;
        }
        /// <summary>
        /// 终止任务
        /// </summary>
        public void EndTask ()
        {
            do
            {
                int status = Interlocked.CompareExchange(ref this._ExecStatus, 3, 4);
                if (status == 4 || status == 2 || status == 3)
                {
                    break;
                }
                else if (Interlocked.CompareExchange(ref this._ExecStatus, 2, status) == status)
                {
                    break;
                }
            } while (true);
        }
        public void Reset (int verNum)
        {
            int status = Interlocked.CompareExchange(ref this._ExecStatus, 0, 1);
            if (status == 1)
            {
                this.VerNum = verNum;
                if (!this._InitTask(false))
                {
                    this.StopTask();
                    return;
                }
                _ = Interlocked.Exchange(ref this._ExecStatus, 1);//启用
            }
        }
    }
}
