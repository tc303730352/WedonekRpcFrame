using System;
using System.Collections.Generic;
using System.Threading;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Lock;
using WeDonekRpc.Helper.Log;

namespace WeDonekRpc.Helper
{
    public delegate void SaveDelayData<T> (ref T[] datas);
    public delegate void FilterSaveData<T> (ref T[] datas);
    public class DelayDataSave<T> : IDelayDataSave<T>
    {
        private static readonly TimeSpan _DefTime = new TimeSpan(0, 1, 0);
        private static readonly int _DefMaxNum = -1;

        private readonly Timer _SaveTimer;

        ~DelayDataSave ()
        {
            this.Dispose();
        }
        public DelayDataSave (SaveDelayData<T> action, int interval, int maxLen) : this(action, null, interval, maxLen)
        {
        }
        public DelayDataSave (SaveDelayData<T> action, FilterSaveData<T> filter, int interval, int maxLen) : this(action, new TimeSpan(0, 0, 0, 0, interval * 1000 / 2))
        {
            this._FilterAction = filter;
            this.MaxDataLen = maxLen;
            this._Interval = interval;
            this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + interval;
        }
        public DelayDataSave (SaveDelayData<T> action, FilterSaveData<T> filter, TimeSpan time) : this(action, time)
        {
            this._FilterAction = filter;
        }
        public DelayDataSave (SaveDelayData<T> action, FilterSaveData<T> filter) : this(action, filter, _DefTime)
        {
        }
        public DelayDataSave (SaveDelayData<T> action, TimeSpan time)
        {
            this._SaveAction = action;
            int mill = (int)time.TotalMilliseconds;
            this._SaveTimer = new Timer(new TimerCallback(this._SaveData), null, mill, mill);
        }



        private volatile int _SaveTime = 0;
        private readonly int _Interval = 0;
        public int MaxDataLen
        {
            get;
            set;
        } = _DefMaxNum;

        private readonly int _MaxRetryNum = 3;

        private readonly SaveDelayData<T> _SaveAction = null;

        private readonly FilterSaveData<T> _FilterAction = null;

        private readonly HashSet<T> _DataList = [];

        private readonly LockHelper _Lock = new LockHelper();
        public void AddData (T data)
        {
            if (this._Lock.GetLock())
            {
                _ = this._DataList.Add(data);
                this._Lock.Exit();
            }
        }
        public void AddData (T[] data)
        {
            if (this._Lock.GetLock())
            {
                foreach (T a in data)
                {
                    _ = this._DataList.Add(a);
                }
                this._Lock.Exit();
            }
        }
        private void _Save ()
        {
            T[] list = null;
            if (this._Lock.GetLock())
            {
                list = new T[this._DataList.Count];
                this._DataList.CopyTo(list, 0);
                this._DataList.Clear();
                this._Lock.Exit();
            }
            if (list != null && list.Length > 0)
            {
                this.FilterSaveData(ref list);
                if (list == null || list.Length == 0)
                {
                    return;
                }
                this.Save(list);
            }
        }
        private void _SaveData (object state)
        {
            int count = this._DataList.Count;
            if (count == 0)
            {
                return;
            }
            else if (this.MaxDataLen != -1 && ( this.MaxDataLen <= count || this._SaveTime <= HeartbeatTimeHelper.HeartbeatTime ))
            {
                this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + this._Interval;
                this._Save();
            }
            else if (this.MaxDataLen == -1)
            {
                this._Save();
            }
        }
        private class _RetryData
        {
            public T[] Datas;

            public int RetryNum;
        }
        protected virtual void FilterSaveData (ref T[] datas)
        {
            this._FilterAction?.Invoke(ref datas);
        }
        private void Save (T[] datas)
        {
            if (!this._Save(ref datas, 0, out string error))
            {
                if (this._MaxRetryNum != 0)
                {
                    _RetryData obj = new _RetryData
                    {
                        Datas = datas
                    };
                    _ = new Timer(new TimerCallback(this._RetrySave), obj, Tools.GetRandom(500, 1200), Timeout.Infinite);
                }
                else
                {
                    this._SaveError(error, datas);
                }
            }
        }
        private void _SaveError (string error, T[] datas)
        {
            new WarnLog(error, "延迟保存数据失败!", string.Empty, "DelayDataSave")
             {
                { "Source", this._SaveAction.Method.DeclaringType.FullName},
                {"DataLen", datas.Length },
                {"DataType", typeof(T).FullName }
              }.Save();
        }
        protected virtual bool _Save (ref T[] datas, int retryNum, out string error)
        {
            try
            {
                this._SaveAction(ref datas);
                error = null;
                return true;
            }
            catch (Exception e)
            {
                ErrorException ex = ErrorException.FormatError(e);
                error = ex.ToString();
                if (ex.IsSystemError)
                {
                    string data = datas.ToJson();
                    ErrorLog log = new ErrorLog(ex, "延迟保存数据错误!", "retry:" + retryNum.ToString(), "DelayDataSave")
                    {
                        { "Source", this._SaveAction.Method.DeclaringType.FullName },
                        { "data", data }
                    };
                    log.IsLocal = true;
                    log.Save();
                }
                return ex.IsEnd;
            }
        }
        private void _RetrySave (object state)
        {
            _RetryData data = (_RetryData)state;
            if (!this._Save(ref data.Datas, data.RetryNum, out string error))
            {
                if (++data.RetryNum < this._MaxRetryNum)
                {
                    int time = 500 + ( 100 * data.RetryNum );
                    _ = new Timer(new TimerCallback(this._RetrySave), data, Tools.GetRandom(500, time), Timeout.Infinite);
                }
                else
                {
                    this._SaveError(error, data.Datas);
                }
            }
        }
        public void SaveData ()
        {
            this._SaveData(null);
        }
        public void Dispose ()
        {
            this._SaveTimer?.Dispose();
            if (this._DataList.Count > 0)
            {
                this._SaveData(null);
            }
        }
    }
}
