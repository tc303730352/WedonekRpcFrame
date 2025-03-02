using System.Threading;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Helper.Lock;

namespace WeDonekRpc.Helper
{
    internal class AutoTask
    {
        private static readonly Timer _ClearTimer;
        private static IAutoTask[] _clearList;
        private static readonly LockHelper _Lock = new LockHelper();
        static AutoTask ()
        {
            _ClearTimer = new Timer(_Clear, null, 1000, 1000);
        }

        private static void _Clear (object state)
        {
            if (_clearList.IsNull())
            {
                return;
            }
            int now = HeartbeatTimeHelper.HeartbeatTime;
            _clearList.ForEach(c =>
            {
                if (c.IsExec(now))
                {
                    c.ExecuteTask();
                }
            });
        }
        public static void Add (IAutoTask task)
        {
            if (_Lock.GetLock())
            {
                _clearList = _clearList.Add(task);
                _Lock.Exit();
            }
        }
    }
}
