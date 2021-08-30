using System;
using System.Collections.Generic;
using System.Threading;

using RpcHelper.TaskTools;

namespace RpcHelper
{
        public delegate void SaveDelayData<T>(ref T[] datas);
        public delegate void FilterSaveData<T>(ref T[] datas);
        public class DelayDataSave<T> : IDelayDataSave<T>
        {
                private static readonly TimeSpan _DefTime = new TimeSpan(0, 1, 0);
                private static readonly int _DefMaxNum = -1;
                ~DelayDataSave()
                {
                        this.Dispose();
                }
                public DelayDataSave(SaveDelayData<T> action, int interval, int maxLen) : this(action, null, interval, maxLen)
                {
                }
                public DelayDataSave(SaveDelayData<T> action, FilterSaveData<T> filter, int interval, int maxLen) : this(action, new TimeSpan(0, 0, interval))
                {
                        this._FilterAction = filter;
                        this.MaxDataLen = maxLen;
                        this._Interval = interval;
                        this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + interval;
                }
                public DelayDataSave(SaveDelayData<T> action, FilterSaveData<T> filter, TimeSpan time) : this(action, time)
                {
                        this._FilterAction = filter;
                }
                public DelayDataSave(SaveDelayData<T> action, FilterSaveData<T> filter) : this(action, filter, _DefTime)
                {
                }
                public DelayDataSave(SaveDelayData<T> action, TimeSpan time)
                {
                        this._SaveAction = action;
                        this._TaskId = TaskManage.AddTask(new TaskHelper(string.Empty, time, this._SaveData, this._SaveData));
                }



                private volatile int _SaveTime = 0;
                private readonly int _Interval = 0;
                public int MaxDataLen
                {
                        get;
                        set;
                } = _DefMaxNum;

                private readonly string _TaskId = null;

                private readonly int _MaxRetryNum = 3;

                private readonly SaveDelayData<T> _SaveAction = null;

                private readonly FilterSaveData<T> _FilterAction = null;

                private readonly HashSet<T> _DataList = new HashSet<T>();

                private readonly LockHelper _Lock = new LockHelper();
                public void AddData(T data)
                {
                        if (this._Lock.GetLock())
                        {
                                this._DataList.Add(data);
                                this._Lock.Exit();
                        }
                }
                public void AddData(T[] data)
                {
                        if (this._Lock.GetLock())
                        {
                                foreach (T a in data)
                                {
                                        this._DataList.Add(a);
                                }
                                this._Lock.Exit();
                        }
                }
                private void _Save()
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
                private void _SaveData()
                {
                        int count = this._DataList.Count;
                        if (count == 0)
                        {
                                return;
                        }
                        else if (this.MaxDataLen != -1 && (this.MaxDataLen <= count || this._SaveTime <= HeartbeatTimeHelper.HeartbeatTime))
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
                protected virtual void FilterSaveData(ref T[] datas)
                {
                        this._FilterAction?.Invoke(ref datas);
                }
                private void Save(T[] datas)
                {
                        if (!this._Save(ref datas, out string error))
                        {
                                if (this._MaxRetryNum != 0)
                                {
                                        _RetryData obj = new _RetryData
                                        {
                                                Datas = datas
                                        };
                                        new Timer(new TimerCallback(this._RetrySave), obj, Tools.GetRandom(1000, 5000), Timeout.Infinite);
                                }
                                else
                                {
                                        this._SaveError(error, datas);
                                }
                        }
                }
                private void _SaveError(string error, T[] datas)
                {
                        if (error != "public.system.error")
                        {
                                new WarnLog(error, "延迟保存数据失败!", datas.ToJson(), "DelayDataSave")
                                {
                                    { "Source",this._SaveAction.Method.DeclaringType.FullName}
                                }.Save();
                        }
                }
                protected virtual bool _Save(ref T[] datas, out string error)
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
                                        new ErrorLog(ex, "延迟保存数据失败!", datas.ToJson(), "DelayDataSave")
                                        {
                                                { "Source",this._SaveAction.Method.DeclaringType.FullName}
                                        }.Save();
                                }
                                return ex.IsEnd;
                        }
                }
                private void _RetrySave(object state)
                {
                        _RetryData data = (_RetryData)state;
                        if (!this._Save(ref data.Datas, out string error))
                        {
                                if (++data.RetryNum < this._MaxRetryNum)
                                {
                                        int time = 5000 + (4000 * data.RetryNum);
                                        new Timer(new TimerCallback(this._RetrySave), data, Tools.GetRandom(1000, time), Timeout.Infinite);
                                }
                                else
                                {
                                        this._SaveError(error, data.Datas);
                                }
                        }
                }
                public void SaveData()
                {
                        this._SaveData();
                }
                public void Dispose()
                {
                        if (this._TaskId != null)
                        {
                                TaskManage.RemoveTask(this._TaskId);
                        }
                        if (this._DataList.Count > 0)
                        {
                                this._SaveData();
                        }
                }
        }
}
