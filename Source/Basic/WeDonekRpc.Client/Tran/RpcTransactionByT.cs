using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.TranService;
using WeDonekRpc.Helper;

namespace WeDonekRpc.Client.Tran
{
    /// <summary>
    /// 声明一个现有事务
    /// </summary>
    /// <typeparam name="T">事务起始数据</typeparam>
    public class RpcTransaction<T> : IRpcTransaction where T : class
    {
        private readonly ICurTranState _Tran;

        private readonly ICurTranState _Source;

        private bool _IsComplate = false;

        private readonly bool _Isinherit = false;

        /// <summary>
        /// 事务构造函数
        /// </summary>
        /// <param name="tranName">事务名</param>
        /// <param name="data">起始数据</param>
        /// <param name="isinherit">是否继承上级事务</param>
        public RpcTransaction(string tranName, T data, bool isinherit = true)
        {
            this._Isinherit = RpcTranService.ApplyTran(tranName, data.ToJson(), isinherit, out this._Tran, out this._Source);
        }
        /// <summary>
        /// 事务构造函数
        /// </summary>
        /// <param name="tranName">事务名</param>
        /// <param name="isinherit">是否继承上级事务</param>
        public RpcTransaction(string tranName, bool isinherit = true)
        {
            this._Isinherit = RpcTranService.ApplyTran(tranName, null, isinherit, out this._Tran, out this._Source);
        }
        /// <summary>
        /// 事务完成
        /// </summary>
        public void Complate()
        {
            if (this._Isinherit)
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
      
        /// <summary>
        /// 销毁事务-未提交自动回滚
        /// </summary>
        public void Dispose()
        {
            if (this._Isinherit)
            {
                return;
            }
            else if (!this._IsComplate)
            {
                RpcTranService.RollbackTran(this._Tran, this._Source);
            }
        }

        public void SetExtend(string extend)
        {
            RpcTranService.SetTranExtend(this._Tran, extend);
        }
    }
}
