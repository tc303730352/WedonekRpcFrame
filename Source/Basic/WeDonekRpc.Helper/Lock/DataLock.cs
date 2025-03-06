using System.Threading;
namespace WeDonekRpc.Helper.Lock
{
    public class DataLock : LockHelper
    {
        private int _UseNum = 0;
        private int _Time = int.MaxValue;
        public DataLock () : base()
        {
        }
        internal bool IsNoUse ( int time )
        {
            return Interlocked.CompareExchange(ref this._UseNum, 0, 0) == 0 && this._Time < time;
        }
        internal void AddUseNum ()
        {
            this._Time = HeartbeatTimeHelper.HeartbeatTime;
            _ = Interlocked.Add(ref this._UseNum, 1);
        }
        public override void Dispose ()
        {
            _ = Interlocked.Add(ref this._UseNum, -1);
            base.Dispose();
        }
    }
}
