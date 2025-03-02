using WeDonekRpc.Helper;

namespace WeDonekRpc.IOBuffer.Buffer
{
    internal class ExpansionBuffer : SocketBuffer
    {
        private volatile int _HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;

        public ExpansionBuffer (int len,int threadId) : base(len, threadId)
        {

        }
        public bool CheckIsOverTime (int time)
        {
            return this._HeartbeatTime < time && !this.IsUsable;
        }
        public override void Dispose ()
        {
            this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            base.Dispose();
        }
        public override void Dispose (int ver)
        {
            this._HeartbeatTime = HeartbeatTimeHelper.HeartbeatTime;
            base.Dispose(ver);
        }
    }
}
