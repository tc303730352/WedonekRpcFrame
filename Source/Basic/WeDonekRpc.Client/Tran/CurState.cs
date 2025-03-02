using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran.Model;

namespace WeDonekRpc.Client.Tran
{
    internal class CurState : ICurTran
    {
        public CurState (TranCommit tran, MsgSource source)
        {
            this.TranState = new CurTranState
            {
                OverTime = tran.OverTime,
                TranId = tran.TranId,
                RegionId = source.RegionId,
                RpcMerId = source.RpcMerId
            };
            this.Body = new TranSource(tran.SubmitJson, tran.Extend);
        }
        public CurState (TranRollback tran, MsgSource source)
        {
            this.TranState = new CurTranState
            {
                OverTime = tran.OverTime,
                TranId = tran.TranId,
                RegionId = source.RegionId,
                RpcMerId = source.RpcMerId
            };
            this.Body = new TranSource(tran.SubmitJson, tran.Extend);
        }

        /// <summary>
        /// 事务ID
        /// </summary>
        public long TranId => this.TranState.TranId;

        public long OverTime => this.TranState.OverTime;

        public ITranSource Body { get; }

        public CurTranState TranState { get; }

        public override bool Equals (object obj)
        {
            if (obj is ICurTran i)
            {
                return i.TranId == this.TranId;
            }
            return false;
        }

        public bool Equals (ICurTran other)
        {
            if (other == null)
            {
                return false;
            }
            return other.TranId == this.TranId;
        }

        public override int GetHashCode ()
        {
            return this.TranId.GetHashCode();
        }
    }
}
