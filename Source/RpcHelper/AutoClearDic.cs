using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcHelper.TaskTools;
namespace RpcHelper
{
        public class AutoClearDic
        {
                public static void AutoClear<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic, int overTime = 600) where TValue : IDataSyncClass
                {
                        TaskManage.AddTask(new AutoClearDic<TKey, TValue>(dic, new TimeSpan(0, 0, Tools.GetRandom(60, 90)), overTime));
                }
                public static void AutoClear<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic, Action<TKey, TValue> action, int overTime = 600) where TValue : IDataSyncClass
                {
                        TaskManage.AddTask(new AutoClearDic<TKey, TValue>(dic, new TimeSpan(0, 0, Tools.GetRandom(60, 90)), overTime, action));
                }
                public static void AutoClear<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic, Action<TKey, TValue> action, Func<TKey, TValue, bool> check, int overTime = 600) where TValue : IDataSyncClass
                {
                        TaskManage.AddTask(new AutoClearDic<TKey, TValue>(dic, new TimeSpan(0, 0, Tools.GetRandom(60, 90)), overTime, check, action));
                }
                public static void AutoClear<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic, Func<TKey, TValue, bool> check, int overTime = 600) where TValue : IDataSyncClass
                {
                        TaskManage.AddTask(new AutoClearDic<TKey, TValue>(dic, new TimeSpan(0, 0, Tools.GetRandom(60, 90)), overTime, check));
                }
        }
        internal class AutoClearDic<TKey, TValue> : ITask where TValue : IDataSyncClass
        {
                private readonly ConcurrentDictionary<TKey, TValue> _Dic = null;

                public string TaskId { get; set; }

                public int TaskPriority { get; set; } = 1;

                public string TaskName { get; set; } = "清理字典";

                public TaskType TaskType { get; set; } = TaskType.间隔任务;

                public TimeSpan ExecInterval { get; set; }
                private readonly Action<TKey, TValue> _Action = null;
                private readonly Func<TKey, TValue, bool> _Check = null;
                public bool IsOnce { get; set; }
                private readonly int _OverTime = 600;
                public AutoClearDic(ConcurrentDictionary<TKey, TValue> dic, TimeSpan time)
                {
                        this._Dic = dic;
                        this.ExecInterval = time;
                }
                public AutoClearDic(ConcurrentDictionary<TKey, TValue> dic, TimeSpan time, int overTime, Action<TKey, TValue> action)
                {
                        this._Action = action;
                        this._OverTime = overTime;
                        this._Dic = dic;
                        this.ExecInterval = time;
                }
                public AutoClearDic(ConcurrentDictionary<TKey, TValue> dic, TimeSpan time, int overTime, Func<TKey, TValue, bool> check)
                {
                        this._Check = check;
                        this._OverTime = overTime;
                        this._Dic = dic;
                        this.ExecInterval = time;
                }
                public AutoClearDic(ConcurrentDictionary<TKey, TValue> dic, TimeSpan time, int overTime, Func<TKey, TValue, bool> check, Action<TKey, TValue> action)
                {
                        this._Action = action;
                        this._Check = check;
                        this._OverTime = overTime;
                        this._Dic = dic;
                        this.ExecInterval = time;
                }
                public AutoClearDic(ConcurrentDictionary<TKey, TValue> dic, TimeSpan time, int overTime)
                {
                        this._OverTime = overTime;
                        this._Dic = dic;
                        this.ExecInterval = time;
                }

                private void _Drop(TKey key)
                {
                        if (this._Dic.TryRemove(key, out TValue val))
                        {
                                val.Dispose();
                                this._Action?.Invoke(key, val);
                        }
                }

                public void ExecuteTask()
                {
                        int time = HeartbeatTimeHelper.HeartbeatTime - this._OverTime;
                        TKey[] keys = this._Dic.Where(a => a.Value.HeartbeatTime <= time).Select(a => a.Key).ToArray();
                        if (keys.Length > 0)
                        {
                                keys.ForEach(a =>
                                {
                                        if (this._Check != null)
                                        {
                                                if (!this._Dic.TryGetValue(a, out TValue val) || !this._Check(a, val))
                                                {
                                                        return;
                                                }
                                        }
                                        this._Drop(a);
                                });
                        }
                }

                public void Dispose()
                {

                }
        }
}
