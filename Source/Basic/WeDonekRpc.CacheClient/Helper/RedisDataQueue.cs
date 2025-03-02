using System;
using System.Threading;

using WeDonekRpc.CacheClient.Redis;

using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Json;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.CacheClient.Helper
{
    /// <summary>
    /// Redis消息队列
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisDataQueue<T> : IDataQueueHelper<T>
    {
        private readonly SyncQueueData<T> _Action;
        private readonly SyncRedisQueue<_QueueData> _QueueList = null;
        private readonly Timer _SyncTimer;

        private readonly SaveQueueFailLog<T> _LogAction = null;
        private int _OverTime = 0;
        /// <summary>
        /// 最大重试数
        /// </summary>
        public int MaxRetryNum { get; set; } = 3;
        /// <summary>
        /// 数据过期时间
        /// </summary>
        public int OverTime { get => this._OverTime; set => this._OverTime = value; }
        public RedisDataQueue ( string name, SyncQueueData<T> action, int dueTime, int period )
        {
            this._Action = action;
            this._QueueList = new SyncRedisQueue<_QueueData>(name);
            this._SyncTimer = new Timer(new TimerCallback(this._ExecQueue), null, dueTime, period);
        }
        public RedisDataQueue ( string name, SyncQueueData<T> action ) : this(name, action, 200, 100)
        {
        }
        public RedisDataQueue ( string name, SyncQueueData<T> action, SaveQueueFailLog<T> log ) : this(name, action, 200, 100)
        {
            this._LogAction = log;
        }
        public RedisDataQueue ( string name, SyncQueueData<T> action, SaveQueueFailLog<T> log, int dueTime, int period ) : this(name, action, dueTime, period)
        {
            this._LogAction = log;
        }


        [Serializable]
        private class _QueueData
        {
            public T Data;
            public string error;
            public int RetryNum;
            public int OverTime;
        }
        private void _ExecQueue ( object state )
        {
            if ( this._QueueList.TryDequeue(out _QueueData data) )
            {
                this._SyncData(data);
            }
        }
        private void _SyncData ( _QueueData data )
        {
            if ( data.OverTime < HeartbeatTimeHelper.HeartbeatTime )
            {
                this._SaveErrorLog(data.Data, data.error, data.RetryNum);
            }
            else if ( !this._ExecAction(data.Data, out string error) )
            {
                data.RetryNum += 1;
                data.error = error;
                if ( this.MaxRetryNum < data.RetryNum )
                {
                    _ = this._QueueList.Enqueue(data);
                }
                else
                {
                    this._SaveErrorLog(data.Data, data.error, data.RetryNum);
                }
            }
        }
        private bool _ExecAction ( T data, out string error )
        {
            try
            {
                this._Action(data);
                error = null;
                return true;
            }
            catch ( Exception e )
            {
                ErrorException ex = ErrorException.FormatError(e);
                new LogInfo(ex, "RedisQueue")
                {
                    LogTitle = "数据处理失败!",
                    LogContent = JsonTools.Json(data)
                }.Save();
                if ( ex.IsEnd )
                {
                    error = null;
                    return true;
                }
                error = ex.ToString();
                return false;
            }
        }
        private void _SaveErrorLog ( T model, string error, int retryNum )
        {
            if ( this._LogAction != null )
            {
                try
                {
                    this._LogAction.Invoke(model, error, retryNum);
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
            _ = this._QueueList.Enqueue(new _QueueData
            {
                Data = data,
                RetryNum = 0,
                OverTime = this._OverTime == 0 ? int.MaxValue : HeartbeatTimeHelper.HeartbeatTime + this._OverTime
            });
        }
        public void AddQueue ( T[] list )
        {
            _QueueData[] datas = list.ConvertAll(a => new _QueueData
            {
                Data = a,
                RetryNum = 0,
                OverTime = this._OverTime == 0 ? int.MaxValue : HeartbeatTimeHelper.HeartbeatTime + this._OverTime
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
