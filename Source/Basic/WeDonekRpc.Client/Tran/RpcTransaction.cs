using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Tran
{
    /// <summary>
    /// 声明一个基础事务
    /// </summary>
    public class RpcTransaction : IRpcTransaction
    {
        /// <summary>
        /// 事务ID
        /// </summary>
        private readonly ICurTranState _Tran;
        /// <summary>
        /// 上级事务Id
        /// </summary>
        private readonly ICurTranState _Source;
        /// <summary>
        /// 是否完成
        /// </summary>
        private bool _IsComplate = false;
        /// <summary>
        /// 是否继承了上级事务
        /// </summary>
        private readonly bool _Isinherit = false;

        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="isinherit">是否继承上级事务</param>
        public RpcTransaction (bool isinherit = true)
        {
            this._Isinherit = RpcTranService.ApplyTran("DefTran", null, isinherit, out this._Tran, out this._Source);
        }
        /// <summary>
        /// 事务
        /// </summary>
        /// <param name="tranName">事务名</param>
        /// <param name="isinherit">是否继承上级事务</param>
        public RpcTransaction (string tranName, bool isinherit = true)
        {
            this._Isinherit = RpcTranService.ApplyTran(tranName, string.Empty, isinherit, out this._Tran, out this._Source);
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Complate ()
        {
            if (this._Isinherit)//子事务不做事务处理
            {
                return;
            }
            else if (!this._IsComplate)
            {
                this._IsComplate = true;
                RpcTranService.SubmitTran(this._Tran, this._Source);
                return;
            }
            throw new ErrorException("rpc.tran.already.submit");
        }
        public void SetExtend (string extend)
        {
            RpcTranService.SetTranExtend(this._Tran, extend);
        }

        /// <summary>
        /// 销毁事务
        /// </summary>
        public void Dispose ()
        {
            if (this._Isinherit)//子事务不做事务处理
            {
                return;
            }
            else if (!this._IsComplate)
            {
                RpcTranService.RollbackTran(this._Tran, this._Source);
            }
        }
    }

}
