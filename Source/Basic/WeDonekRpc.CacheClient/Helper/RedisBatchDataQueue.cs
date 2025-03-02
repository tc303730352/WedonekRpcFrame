using System;
using System.Threading;

using WeDonekRpc.CacheClient.Redis;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.CacheClient.Helper
{
    public delegate void SaveQueue<T> ( QueueData<T>[] data, Action<QueueData<T>, ErrorException> fail );

    public delegate void SaveQueueFail<T> ( T data, ErrorException error, int retryNum );
    [Serializable]
    public class QueueData<T>
    {
        public T value;
        internal int retry;
    }
    /// <summary>
    /// Redis消息队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisBatchDataQueue<T> : IDataQueueHelper<T>
    {
        private readonly SaveQueue<T> _Action;
        private readonly SyncRedisQueue<QueueData<T>> _QueueList = null;
        private readonly Timer _SyncTimer;

        private readonly SaveQueueFail<T> _LogAction = null;

        /// <summary>
        /// 最大重试数
        /// </summary>
        public int MaxRetryNum { get; set; } = 3;

        /// <summary>
        /// 每次取队列中的数量
        /// </summary>
        public int EveryNum { get; } = 1;
        public RedisBatchDataQueue ( string name, SaveQueue<T> action, int dueTime, int period, int every = 5 )
        {
            this._Action = action;
            this.EveryNum = every;
            this._QueueList = new SyncRedisQueue<QueueData<T>>(name);
            this._SyncTimer = new Timer(new TimerCallback(this._ExecQueue), null, dueTime, period);
        }
        public RedisBatchDataQueue ( string name, SaveQueue<T> action, int every = 5 ) : this(name, action, 200, 100, every)
        {
        }
        public RedisBatchDataQueue ( string name, SaveQueue<T> action, SaveQueueFail<T> log, int every = 5 ) : this(name, action, 200, 100, every)
        {
            this._LogAction = log;
        }
        public RedisBatchDataQueue ( string name, SaveQueue<T> action, SaveQueueFail<T> log, int dueTime, int period, int every = 5 ) : this(name, action, dueTime, period, every)
        {
            this._LogAction = log;
        }


        private void _ExecQueue ( object state )
        {
            QueueData<T>[] data = this._QueueList.Dequeue(this.EveryNum);
            if ( !data.IsNull() )
            {
                this._SyncData(data);
            }
        }
        private void _SyncData ( QueueData<T>[] data )
        {
            try
            {
                this._Action(data, this._execFail);
            }
            catch ( Exception e )
            {
                ErrorException ex = ErrorException.FormatError(e);
                new LogInfo(ex, "RedisQueue")
                {
                    LogTitle = "数据处理失败!",
                    LogContent = JsonTools.Json(data)
                }.Save();
            }
        }
        private void _execFail ( QueueData<T> data, ErrorException error )
        {
            data.retry += 1;
            if ( this.MaxRetryNum < data.retry )
            {
                _ = this._QueueList.Enqueue(data);
            }
            else
            {
                this._SaveErrorLog(data.value, error, data.retry);
            }
        }
        private void _SaveErrorLog ( T model, ErrorException error, int retryNum )
        {
            if ( this._LogAction != null )
            {
                try
                {
                    this._LogAction(model, error, retryNum);
                }
                catch ( Exception e )
                {
                    ErrorException ex = ErrorException.FormatError(e);
                    new LogInfo(ex, "RedisQueue")
                    {
                        LogTitle = "数据处理日志保存失败!",
                        LogContent = JsonTools.Json(model)
                    }.Save();
                }
            }
        }
        public void AddQueue ( T data )
        {
            _ = this._QueueList.Enqueue(new QueueData<T>
            {
                value = data,
                retry = 0
            });
        }
        public void AddQueue ( T[] list )
        {
            QueueData<T>[] datas = list.ConvertAll(a => new QueueData<T>
            {
                value = a,
                retry = 0
            });
            _ = this._QueueList.Enqueue(datas);
        }

        public void Dispose ()
        {
            this._SyncTimer.Dispose();
        }

        public void SetTimer ( int dueTime, int period )
        {
            _ = this._SyncTimer.Change(dueTime, period);
        }

    }
}
