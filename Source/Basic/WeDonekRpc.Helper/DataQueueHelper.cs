using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper
{
    public delegate void SyncQueueData<T> (T data);

    public delegate void SaveQueueFailLog<T> (T data, string error, int retryNum);
    public class DataQueueHelper<T> : IDataQueueHelper<T>
    {
        ~DataQueueHelper ()
        {
            this.Dispose();
        }

        public DataQueueHelper (int dueTime, int period)
        {
            this._SyncTimer = new Timer(new TimerCallback(this._ExecQueue), null, dueTime, period);
            Process pro = Process.GetCurrentProcess();
            pro.EnableRaisingEvents = true;
            pro.Exited += new EventHandler(this._Pro_Exited);
        }
        public void SetTimer (int dueTime, int period)
        {
            _ = this._SyncTimer.Change(dueTime, period);
        }
        public DataQueueHelper (SyncQueueData<T> action, SaveQueueFailLog<T> log) : this(action)
        {
            this._LogAction = log;
        }
        public DataQueueHelper (SyncQueueData<T> action, SaveQueueFailLog<T> log, int dueTime, int period) : this(action, dueTime, period)
        {
            this._LogAction = log;
        }
        public DataQueueHelper (SyncQueueData<T> action) : this(1000, 100)
        {
            this._Action = action;
        }
        public DataQueueHelper (SyncQueueData<T> action, int dueTime, int period) : this(dueTime, period)
        {
            this._Action = action;
        }
        private void _Pro_Exited (object sender, EventArgs e)
        {
            this.Dispose();
        }

        private int _OverTime = 0;

        private int _RetryTime = 100;

        private readonly SyncQueueData<T> _Action = null;
        private readonly SaveQueueFailLog<T> _LogAction = null;
        private int _MaxRetryNum = 3;
        private readonly ConcurrentQueue<T> _QueueList = new ConcurrentQueue<T>();

        private readonly Timer _SyncTimer = null;

        public int RetryTime { get => this._RetryTime; set => this._RetryTime = value; }
        public int OverTime { get => this._OverTime; set => this._OverTime = value; }
        public int MaxRetryNum { get => this._MaxRetryNum; set => this._MaxRetryNum = value; }

        public void AddQueue (T data)
        {
            this._QueueList.Enqueue(data);
        }
        public void AddOnlyQueue (T data)
        {
            if (this._QueueList.Contains(data))
            {
                return;
            }
            this._QueueList.Enqueue(data);
        }
        private void _ExecQueue (object state)
        {
            if (this._QueueList.TryDequeue(out T data))
            {
                this.SyncData(data);
            }
        }
        private class _RetryData
        {
            public T Data;
            public string error;

            public int RetryNum;

            public int OverTime;
        }
        private bool _InvokeAction (T data, out string error)
        {
            try
            {
                this._Action.Invoke(data);
                error = null;
                return true;
            }
            catch (Exception e)
            {
                if (e is ErrorException i)
                {
                    error = i.ToString();
                    return i.IsEnd;
                }
                LogInfo log = LogSystem.CreateErrorLog(e, "队列数据处理失败！", "DataQueue");
                log.LogContent = JsonTools.Json(data);
                log.Save();
                error = "public.system.error";
                return false;
            }
        }
        protected virtual void SyncData (T data)
        {
            if (this._Action != null)
            {
                if (!this._InvokeAction(data, out string error))
                {
                    if (this._MaxRetryNum != 0)
                    {
                        _RetryData obj = new _RetryData
                        {
                            Data = data,
                            error = error,
                            OverTime = this._OverTime == 0 ? int.MaxValue : HeartbeatTimeHelper.HeartbeatTime + this._OverTime
                        };
                        _ = new Timer(new TimerCallback(this._RetrySync), obj, this._RetryTime, Timeout.Infinite);
                    }
                    else
                    {
                        this._SaveErrorLog(data, error, 0);
                    }
                }
            }
        }
        private void _SaveErrorLog (T model, string error, int retryNum)
        {
            if (this._LogAction != null)
            {
                try
                {
                    this._LogAction.Invoke(model, error, retryNum);
                }
                catch (Exception e)
                {
                    LogInfo log = LogSystem.CreateErrorLog(e, "队列数据处理失败！", "DataQueue");
                    log.LogContent = JsonTools.Json(model);
                    log.Save();
                }
            }
        }
        private void _RetrySync (object state)
        {
            _RetryData data = (_RetryData)state;
            if (data.OverTime <= HeartbeatTimeHelper.HeartbeatTime)
            {
                this._SaveErrorLog(data.Data, data.error, data.RetryNum);
                return;
            }
            try
            {
                if (!this._InvokeAction(data.Data, out string error))
                {
                    if (++data.RetryNum < this._MaxRetryNum && data.OverTime >= HeartbeatTimeHelper.HeartbeatTime)
                    {
                        _ = new Timer(new TimerCallback(this._RetrySync), data, this._RetryTime, Timeout.Infinite);
                    }
                    else
                    {
                        this._SaveErrorLog(data.Data, error, data.RetryNum);
                    }
                }
            }
            catch (Exception e)
            {
                LogInfo log = LogSystem.CreateErrorLog(e, "队列数据处理失败！", "DataQueue");
                log.LogContent = JsonTools.Json(data.Data);
                log.Save();
            }
        }
        public void Dispose ()
        {
            this._SyncTimer?.Dispose();
            if (!this._QueueList.IsEmpty)
            {
                do
                {
                    this._ExecQueue(null);
                } while (!this._QueueList.IsEmpty);
            }
        }

        public void AddQueue (T[] list)
        {
            list.ForEach(a =>
            {
                this._QueueList.Enqueue(a);
            });
        }
    }
}
