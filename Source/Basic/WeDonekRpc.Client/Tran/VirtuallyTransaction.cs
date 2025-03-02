using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Model.Tran.Model;
namespace WeDonekRpc.Client.Tran
{
    internal class VirtuallyTransaction : IRpcVirtuallyTransaction
    {
        private ICurTranState _Tran;
        private readonly CurTranState _TranState;
        private readonly ITranSource _Body;
        public VirtuallyTransaction (CurTranState state, string body)
        {
            this._TranState = state;
            this._Body = new TranSource(body);
        }

        public int RegionId => this._TranState.RegionId;

        public long RpcMerId => this._TranState.RpcMerId;

        public ITranTemplate Template => this._Tran.Template;

        public CurTranState Source => this._TranState;

        public long TranId => this._Tran.TranId;

        public long OverTime => this._TranState.OverTime;

        public ITranSource Body => this._Body;

        public CurTranState TranState => this._TranState;

        public void BeginTran ()
        {

        }

        public void Dispose ()
        {
            this._Tran?.Dispose();
        }

        public bool Equals (ICurTran other)
        {
            if (this._Tran == null)
            {
                return false;
            }
            return other.TranId == this._Tran.TranId;
        }

        public void InitTran (string tranName)
        {
            if (this._Tran != null)
            {
                this._Tran = RpcTranService.AddTranLog(this._TranState, this._Body, tranName);
            }
        }
    }
}
