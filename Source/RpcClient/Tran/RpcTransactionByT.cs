using System;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;
using RpcModel.Tran.Model;

using RpcHelper;

namespace RpcClient.Tran
{
        /// <summary>
        /// 声明一个现有事务
        /// </summary>
        /// <typeparam name="T">事务起始数据</typeparam>
        public class RpcTransaction<T> : IRpcTransaction where T : class
        {
                private readonly CurTranState _Tran;

                private readonly CurTranState _Source;

                private bool _IsComplate = false;

                private readonly bool _Isinherit = false;

                private ICurTran _CurTran = null;
                /// <summary>
                /// 事务
                /// </summary>
                public ICurTran Tran
                {
                        get
                        {
                                if (_CurTran == null)
                                {
                                        _CurTran = _Tran.ConvertMap<CurTranState, CurTran>();
                                }
                                return _CurTran;
                        }
                }

                /// <summary>
                /// 事务构造函数
                /// </summary>
                /// <param name="tranName">事务名</param>
                /// <param name="data">起始数据</param>
                /// <param name="isinherit">是否继承上级事务</param>
                public RpcTransaction(string tranName, T data, TranLevel level = TranLevel.RPC消息, bool isinherit = true)
                {
                        this._Isinherit = RpcTranCollect.ApplyTran(tranName, level, data.ToJson(), isinherit, out this._Tran, out this._Source);
                }
                /// <summary>
                /// 事务构造函数
                /// </summary>
                /// <param name="tranName">事务名</param>
                /// <param name="isinherit">是否继承上级事务</param>
                public RpcTransaction(string tranName, TranLevel level = TranLevel.RPC消息, bool isinherit = true)
                {
                        this._Isinherit = RpcTranCollect.ApplyTran(tranName, level, null, isinherit, out this._Tran, out this._Source);
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
                                RpcTranCollect.SubmitTran(this._Tran, this._Source);
                                return;
                        }
                        throw new ErrorException("rpc.tran.already.submit");
                }
                public void Rollback()
                {
                        RpcTranCollect.ForceRollbackTran(this._Tran);
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
                                RpcTranCollect.RollbackTran(this._Tran, this._Source);
                        }
                }
                public TranStatus Complate(Action<IRpcTran> checkTran, int waitTime = 10)
                {
                        this.Complate();
                        if (this._Tran.Level == TranLevel.RPC消息)
                        {
                                return TranStatus.提交;
                        }
                        return RpcTranCollect.WaitEnd(this._Tran, checkTran, waitTime);
                }
                public void SetExtend(string extend)
                {
                        RpcTranCollect.SetTranExtend(this._Tran, extend);
                }
        }
}
