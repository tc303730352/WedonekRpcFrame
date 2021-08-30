using System;
using System.Threading;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;
using RpcModel.Tran.Model;

using RpcHelper;

namespace RpcClient.Tran
{
        /// <summary>
        /// 声明一个基础事务
        /// </summary>
        public class RpcTransaction : IRpcTransaction
        {
                /// <summary>
                /// 事务ID
                /// </summary>
                private readonly CurTranState _Tran;
                /// <summary>
                /// 上级事务Id
                /// </summary>
                private readonly CurTranState _Source;
                /// <summary>
                /// 是否完成
                /// </summary>
                private bool _IsComplate = false;
                /// <summary>
                /// 是否继承了上级事务
                /// </summary>
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
                /// 事务
                /// </summary>
                /// <param name="level">事务级别</param>
                /// <param name="isinherit">是否继承上级事务</param>
                public RpcTransaction(TranLevel level = TranLevel.RPC消息, bool isinherit = true)
                {
                        this._Isinherit = RpcTranCollect.ApplyTran(isinherit, level, out this._Tran, out this._Source);
                }
                /// <summary>
                /// 事务
                /// </summary>
                /// <param name="tranName">事务名</param>
                /// <param name="isinherit">是否继承上级事务</param>
                public RpcTransaction(string tranName, TranLevel level = TranLevel.RPC消息, bool isinherit = true)
                {
                        this._Isinherit = RpcTranCollect.ApplyTran(tranName, level, string.Empty, isinherit, out this._Tran, out this._Source);
                }
                /// <summary>
                /// 提交事务
                /// </summary>
                public void Complate()
                {
                        if (this._Isinherit)//子事务不做事务处理
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
                public void SetExtend(string extend)
                {
                        RpcTranCollect.SetTranExtend(this._Tran, extend);
                }

                /// <summary>
                /// 销毁事务
                /// </summary>
                public void Dispose()
                {
                        if (this._Isinherit)//子事务不做事务处理
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
                        if (_Tran.Level == TranLevel.RPC消息)
                        {
                                return TranStatus.提交;
                        }
                        return RpcTranCollect.WaitEnd(_Tran, checkTran, waitTime);

                }

                public void Rollback()
                {
                        RpcTranCollect.ForceRollbackTran(this._Tran);
                }
        }

}
