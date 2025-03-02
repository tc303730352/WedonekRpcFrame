using System;
using System.Collections.Generic;
using System.Threading;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.CacheClient.Helper
{
    /// <summary>
    /// Redis延迟保存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisDelayDataSave<T> : IDelayDataSave<T>
    {
        private readonly Redis.SyncRedisQueue<T> _QueueList = null;
        private readonly SaveDelayData<T> _SaveAction;
        private readonly Timer _SaveTimer;
        private readonly FilterSaveData<T> _FilterAction;

        public int MaxDataLen { get; }

        private readonly int _interval;
        private int _SaveTime;
        private static readonly TimeSpan _DefTime = new TimeSpan(0, 1, 0);

        public RedisDelayDataSave (string name, SaveDelayData<T> action, int interval, int maxLen) : this(name, action, null, interval, maxLen)
        {
        }
        public RedisDelayDataSave (string name, SaveDelayData<T> action, FilterSaveData<T> filter, int interval, int maxLen) : this(name, action, new TimeSpan(0, 0, 0, 0, interval * 1000 / 2))
        {
            this._FilterAction = filter;
            this.MaxDataLen = maxLen;
            this._interval = interval;
            this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + interval;
        }
        public RedisDelayDataSave (string name, SaveDelayData<T> action, FilterSaveData<T> filter, TimeSpan time) : this(name, action, time)
        {
            this._FilterAction = filter;
        }
        public RedisDelayDataSave (string name, SaveDelayData<T> action, FilterSaveData<T> filter) : this(name, action, filter, _DefTime)
        {
        }
        public RedisDelayDataSave (string name, SaveDelayData<T> action, TimeSpan time)
        {
            this._QueueList = new Redis.SyncRedisQueue<T>(name);
            this._SaveAction = action;
            int mill = (int)time.TotalMilliseconds;
            this._SaveTimer = new Timer(new TimerCallback(this._SaveData), null, mill, mill);
        }

        private void _SaveData (object state)
        {
            long count = this._QueueList.GetCount();
            if (count == 0)
            {
                return;
            }
            else if (this.MaxDataLen != -1 && ( this.MaxDataLen <= count || this._SaveTime <= HeartbeatTimeHelper.HeartbeatTime ))
            {
                this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + this._interval;
                this._Save(count);
            }
            else if (this.MaxDataLen == -1)
            {
                this._Save(count);
            }
        }
        private void _Save (long count)
        {
            if (count == 1)
            {
                if (this._QueueList.TryDequeue(out T data))
                {
                    this._Save(new T[] { data });
                }
            }
            else
            {
                List<T> list = [];
                do
                {
                    if (this._QueueList.TryDequeue(out T data))
                    {
                        list.Add(data);
                    }
                    else
                    {
                        break;
                    }
                } while (true);
                if (list.Count > 0)
                {
                    this._SaveData(list.ToArray());
                }
            }
        }
        private void _Save (T[] list)
        {
            try
            {
                this._SaveAction(ref list);
            }
            catch (Exception e)
            {
                if (!this._WriteLog(e, list))
                {
                    _ = this._QueueList.Enqueue(list);
                }
            }
        }
        private bool _WriteLog (Exception e, T[] list)
        {
            Type type = typeof(T);
            ErrorException ex = ErrorException.FormatError(e);
            ex.Args.Add("Type", type.FullName);
            ex.Args.Add("DataLength", list.Length.ToString());
            ex.SaveLog("RedisDelayDataSave");
            ErrorLog error = new ErrorLog(ex, "数据延迟保存失败!", "RedisDelayDataSave")
            {
                IsLocal = true,
            };
            error.Add("DataLength", list.Length.ToString());
            error.Add("Type", type.FullName);
            error.Add("data", list.ToJson());
            error.Save();
            return ex.IsEnd;
        }
        private void _SaveData (T[] list)
        {
            this._FilterAction?.Invoke(ref list);
            if (list == null || list.Length == 0)
            {
                return;
            }
            this._Save(list);
        }
        public void AddData (T data)
        {
            _ = this._QueueList.Enqueue(data);
        }

        public void Dispose ()
        {
            this._SaveTimer.Dispose();
        }

        public void SaveData ()
        {
            long count = this._QueueList.GetCount();
            if (count == 0)
            {
                return;
            }
            this._Save(count);
        }

        public void AddData (T[] data)
        {
            _ = this._QueueList.Enqueue(data);
        }
    }
}
