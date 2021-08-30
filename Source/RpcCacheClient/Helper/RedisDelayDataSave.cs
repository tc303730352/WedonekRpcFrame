using System;
using System.Collections.Generic;

using RpcHelper;
using RpcHelper.TaskTools;

namespace RpcCacheClient.Helper
{
        public class RedisDelayDataSave<T> : IDelayDataSave<T>
        {
                private readonly Redis.SyncRedisQueue<T> _QueueList = null;
                private readonly SaveDelayData<T> _SaveAction;
                private readonly string _TaskId;
                private readonly FilterSaveData<T> _FilterAction;

                public int MaxDataLen { get; }

                private readonly int _interval;
                private int _SaveTime;
                private static readonly TimeSpan _DefTime = new TimeSpan(0, 1, 0);

                public RedisDelayDataSave(string name, SaveDelayData<T> action, int interval, int maxLen) : this(name, action, null, interval, maxLen)
                {
                }
                public RedisDelayDataSave(string name, SaveDelayData<T> action, FilterSaveData<T> filter, int interval, int maxLen) : this(name, action, new TimeSpan(0, 0, interval))
                {
                        this._FilterAction = filter;
                        this.MaxDataLen = maxLen;
                        this._interval = interval;
                        this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + interval;
                }
                public RedisDelayDataSave(string name, SaveDelayData<T> action, FilterSaveData<T> filter, TimeSpan time) : this(name, action, time)
                {
                        this._FilterAction = filter;
                }
                public RedisDelayDataSave(string name, SaveDelayData<T> action, FilterSaveData<T> filter) : this(name, action, filter, _DefTime)
                {
                }
                public RedisDelayDataSave(string name, SaveDelayData<T> action, TimeSpan time)
                {
                        this._QueueList = new Redis.SyncRedisQueue<T>(name);
                        this._SaveAction = action;
                        this._TaskId = TaskManage.AddTask(new TaskHelper(string.Empty, time, new Action(this._SaveData)));
                }

                private void _SaveData()
                {
                        long count = this._QueueList.GetCount();
                        if (count == 0)
                        {
                                return;
                        }
                        else if (this.MaxDataLen != -1 && (this.MaxDataLen <= count || this._SaveTime <= HeartbeatTimeHelper.HeartbeatTime))
                        {
                                this._SaveTime = HeartbeatTimeHelper.HeartbeatTime + this._interval;
                                this._Save(count);
                        }
                        else if (this.MaxDataLen == -1)
                        {
                                this._Save(count);
                        }
                }
                private void _Save(long count)
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
                                List<T> list = new List<T>();
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
                private void _Save(T[] list)
                {
                        try
                        {
                                this._SaveAction(ref list);
                        }
                        catch (Exception e)
                        {
                                ErrorException ex = ErrorException.FormatError(e);
                                new LogInfo(ex, "RedisDelayDataSave")
                                {
                                        LogTitle = "数据延迟保存失败!",
                                        LogContent = list.ToJson()
                                }.Save();
                                if (!ex.IsEnd)
                                {
                                        this._QueueList.Enqueue(list);
                                }
                        }
                }
                private void _SaveData(T[] list)
                {
                        this._FilterAction?.Invoke(ref list);
                        if (list == null || list.Length == 0)
                        {
                                return;
                        }
                        this._Save(list);
                }
                public void AddData(T data)
                {
                        this._QueueList.Enqueue(data);
                }

                public void Dispose()
                {
                        TaskManage.RemoveTask(this._TaskId);
                }

                public void SaveData()
                {
                        long count = this._QueueList.GetCount();
                        if (count == 0)
                        {
                                return;
                        }
                        this._Save(count);
                }

                public void AddData(T[] data)
                {
                        this._QueueList.Enqueue(data);
                }
        }
}
