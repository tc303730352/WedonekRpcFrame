using RpcSync.Collect;
using RpcSync.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Model;
using WeDonekRpc.Model.Tran;
using WeDonekRpc.Model.Tran.Model;
namespace RpcSync.Service.Event
{
    /// <summary>
    /// 远程事务模块
    /// </summary>
    internal class RpcTranEvent : IRpcApiService
    {
        private readonly IRpcTransactionService _Service;
        private readonly ITransactionCollect _Tran;
        public RpcTranEvent (IRpcTransactionService service, ITransactionCollect tran)
        {
            this._Tran = tran;
            this._Service = service;
        }


        /// <summary>
        /// 检查超时的事务
        /// </summary>
        public void CheckOverTimeTran ()
        {
            this._Service.CheckOverTimeTran();
        }
        /// <summary>
        /// 获取事务结果
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TranResult GetTranResult (GetTranResult obj)
        {
            return this._Service.GetTranResult(obj.TranId);
        }

        /// <summary>
        /// 重启失败的事务
        /// </summary>
        public void RestartRetryTran ()
        {
            this._Service.RestartRetryTran();
        }
        /// <summary>
        /// 锁事务超时
        /// </summary>
        public void TranLockOverTime ()
        {
            this._Service.TranLockOverTime();
        }
        /// <summary>
        /// 申请事务
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="source"></param>
        public void ApplyTran (ApplyTran obj, MsgSource source)
        {
            this._Service.ApplyTransaction(source, obj);
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <param name="obj"></param>
        public void RollbackTran (RollbackTran obj)
        {
            this._Service.RollbackTran(obj.TranId);
        }
        /// <summary>
        /// 设置事务的扩展参数
        /// </summary>
        /// <param name="obj"></param>
        public void SetTranExtend (SetTranExtend obj)
        {
            this._Tran.SetTranExtend(obj.TranId, obj.Extend);
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        /// <param name="obj"></param>
        public void SubmitTran (SubmitTran obj)
        {
            this._Service.SubmitTran(obj.TranId);
        }
        /// <summary>
        /// 获取事务状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TransactionStatus GetTranState (GetTranState obj)
        {
            return this._Tran.GetTranState(obj.TranId).TranStatus;
        }


        /// <summary>
        /// 添加事务日志
        /// </summary>
        /// <param name="add"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public long AddTranLog (AddTranLog add, MsgSource source)
        {
            return this._Service.AddTranLog(add.TranId, add.TranLog, source);
        }
    }
}
