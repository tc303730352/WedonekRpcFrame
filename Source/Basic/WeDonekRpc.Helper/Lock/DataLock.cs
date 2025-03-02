using System.Threading;
namespace WeDonekRpc.Helper.Lock
{
    public class DataLock : LockHelper
    {
        private int _UseNum = 0;
        private int _Time = int.MaxValue;
        public DataLock():base()
        {
        }
        internal bool IsNoUse(int time)
        {
            return Interlocked.CompareExchange(ref _UseNum, 0, 0) == 0 && _Time < time;
        }
        internal void AddUseNum()
        {
            _Time = HeartbeatTimeHelper.HeartbeatTime;
            Interlocked.Add(ref this._UseNum, 1);
        }
        public override void Dispose()
        {
            Interlocked.Add(ref _UseNum, -1);
            base.Dispose();
        }
    }
}
