using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Model.Tran.Model;
namespace WeDonekRpc.Client.Tran
{
    internal class VirtuallyTransaction : IRpcVirtuallyTransaction
    {
        private ICurTranState _Tran;

        public VirtuallyTransaction ( CurTranState state, string body )
        {
            this.TranState = state;
            this.Body = new TranSource(body);
        }

        public int RegionId => this.TranState.RegionId;

        public long RpcMerId => this.TranState.RpcMerId;

        public ITranTemplate Template => this._Tran.Template;

        public CurTranState Source => this.TranState;

        public long TranId => this._Tran.TranId;

        public long OverTime => this.TranState.OverTime;

        public ITranSource Body { get; }

        public CurTranState TranState { get; }

        public bool IsDispose => this._Tran.IsDispose;

        public void BeginTran ()
        {

        }

        public void Dispose ()
        {
            this._Tran?.Dispose();
        }

        public bool Equals ( ICurTran other )
        {
            if ( this._Tran == null )
            {
                return false;
            }
            return other.TranId == this._Tran.TranId;
        }

        public void InitTran ( string tranName )
        {
            if ( this._Tran != null )
            {
                this._Tran = RpcTranService.AddTranLog(this.TranState, this.Body, tranName);
            }
        }
    }
}
