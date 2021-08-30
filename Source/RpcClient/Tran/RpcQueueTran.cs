using System;

using RpcClient.Collect;
using RpcClient.Interface;
using RpcClient.Model;

using RpcModel;
using RpcModel.Tran.Model;

namespace RpcClient.Tran
{
        internal class RpcQueueTran : IRpcTransaction
        {
                /// <summary>
                /// 事务
                /// </summary>
                private readonly CurTranState _Tran;
                /// <summary>
                /// 是否完成
                /// </summary>
                private bool _IsComplate = false;

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
                /// <param name="isinherit">是否继承上级事务</param>
                public RpcQueueTran(QueueRemoteMsg msg)
                {
                        this._Tran = RpcTranCollect.ApplyQueueTran(msg);
                }

                /// <summary>
                /// 提交事务
                /// </summary>
                public void Complate()
                {
                        if (this._IsComplate)
                        {
                                return;
                        }
                        this._IsComplate = true;
                        RpcTranCollect.SubmitTran(this._Tran, null);
                }
                public void SetExtend(string extend)
                {
                        RpcTranCollect.SetTranExtend(this._Tran, extend);
                }
                public TranStatus Complate(Action<IRpcTran> checkTran, int waitTime = 10)
                {
                        this.Complate();
                        return TranStatus.提交;
                }
                /// <summary>
                /// 销毁事务
                /// </summary>
                public void Dispose()
                {
                        if (!this._IsComplate)
                        {
                                RpcTranCollect.RollbackTran(this._Tran);
                        }
                }

                public void Rollback()
                {
                        RpcTranCollect.ForceRollbackTran(this._Tran);
                }
        }
}
