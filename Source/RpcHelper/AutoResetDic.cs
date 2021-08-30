using System;
using System.Collections.Concurrent;
using System.Linq;

using RpcHelper.TaskTools;

namespace RpcHelper
{
        public class AutoResetDic
        {
                public static void AutoReset<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dic, int overTime = 600) where TValue : IDataSyncClass
                {
                        TaskManage.AddTask(new AutoResetDic<TKey, TValue>(dic, new TimeSpan(0, 0, Tools.GetRandom(60, 90)), overTime));
                }
        }
        internal class AutoResetDic<TKey, TValue> : ITask where TValue : IDataSyncClass
        {
                private readonly ConcurrentDictionary<TKey, TValue> _Dic = null;

                public string TaskId { get; set; }

                public int TaskPriority { get; set; } = 1;

                public string TaskName { get; set; } = "清理字典";

                public TaskType TaskType { get; set; } = TaskType.间隔任务;

                public TimeSpan ExecInterval { get; set; }
                public bool IsOnce { get; set; }
                private readonly int _OverTime = 60;

                public AutoResetDic(ConcurrentDictionary<TKey, TValue> dic, TimeSpan time, int overTime)
                {
                        this._OverTime = overTime;
                        this._Dic = dic;
                        this.ExecInterval = time;
                }


                private void _Reset(TKey key)
                {
                        if (this._Dic.TryGetValue(key, out TValue val))
                        {
                                val.ResetLock();
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
                                        this._Reset(a);
                                });
                        }
                }

                public void Dispose()
                {

                }
        }
}
