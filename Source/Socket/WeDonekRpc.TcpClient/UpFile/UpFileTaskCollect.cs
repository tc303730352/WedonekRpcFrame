using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

using WeDonekRpc.Helper;

namespace WeDonekRpc.TcpClient.UpFile
{
    internal class UpFileTaskCollect
    {
        static UpFileTaskCollect()
        {
            _ = new Timer(new TimerCallback(_CheckTimeout), null, 1000, 1000);
        }

        private static readonly ConcurrentDictionary<string, UpFileTask> _UpTaskList = new ConcurrentDictionary<string, UpFileTask>();

        private static void _CheckTimeout(object state)
        {
            if (_UpTaskList.Count == 0)
            {
                return;
            }
            string[] keys = _UpTaskList.Keys.ToArray();
            int sendTimeOut = HeartbeatTimeHelper.HeartbeatTime - Config.SocketConfig.UpFileSendTimeOut;
            keys.ForEach(a =>
            {
                if (_UpTaskList.TryGetValue(a, out UpFileTask task))
                {
                    task.CheckUpState(sendTimeOut);
                }
            });
        }


        public static bool GetOrAddTask(ref UpFileTask task)
        {
            task = _UpTaskList.GetOrAdd(task.TaskId, task);
            if (!task.Init())
            {
                _UpTaskList.TryRemove(task.TaskId, out task);
                task.Dispose();
                return false;
            }
            return task.IsInit;
        }
        public static void CacnelTask(string taskId)
        {
            if (_UpTaskList.TryRemove(taskId, out UpFileTask task))
            {
                task.Cancel();
            }
        }
        public static void RemoveTask(string taskId)
        {
            if (_UpTaskList.TryRemove(taskId, out UpFileTask task))
            {
                task.Dispose();
            }
        }
    }
}
