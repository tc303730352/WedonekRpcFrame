using RpcTaskModel;
using WeDonekRpc.CacheClient.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;

namespace AutoTask.Collect.LocalEvent.Event
{
    [LocalEventName("refresh", "stopTask", "setTaskTime", "execBegin", "execEnd", "status", "delete")]
    internal class ClearTaskCacheEvent : IEventHandler<AutoTaskEvent>
    {
        private readonly ICacheController _Cache;
        public ClearTaskCacheEvent (ICacheController cache)
        {
            this._Cache = cache;
        }
        public void HandleEvent (AutoTaskEvent data, string eventName)
        {
            string key = string.Concat("TaskState_", data.TaskId);
            _ = this._Cache.Remove(key);
            if (eventName == "status" && data.Task.TaskStatus == AutoTaskStatus.启用)
            {
                key = string.Join("_", "Task", data.TaskId, data.Task.VerNum);
                _ = this._Cache.Remove(key);
                key = string.Join("_", "TaskItem", data.TaskId, data.Task.VerNum);
                _ = this._Cache.Remove(key);
                key = string.Join("_", "TaskPlan", data.TaskId, data.Task.VerNum);
                _ = this._Cache.Remove(key);
            }
        }
    }
}
