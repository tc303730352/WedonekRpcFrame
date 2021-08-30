using System;

using RpcModel;
using RpcModel.Tran;
using RpcModel.Tran.Model;

using RpcSyncService.Collect;
using RpcSyncService.Logic;

namespace RpcSyncService.Event
{
        /// <summary>
        /// 远程事务模块
        /// </summary>
        internal class RpcTranEvent : RpcClient.Interface.IRpcApiService
        {
                /// <summary>
                /// 检查超时的事务
                /// </summary>
                public void CheckOverTimeTran()
                {
                        RpcTranLogic.CheckOverTimeTran();
                }
                /// <summary>
                /// 获取事务结果
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public TranResult GetTranResult(GetTranResult obj)
                {
                        return RpcTranLogic.GetTranResult(obj.TranId);
                }
                /// <summary>
                /// 检查挂起的事务状态
                /// </summary>
                public void CheckHangUpTran()
                {
                        RpcTranLogic.CheckHangUpTran();
                }
                /// <summary>
                /// 重启回滚失败的事务
                /// </summary>
                public void RestartRetryTran()
                {
                        RpcTranLogic.RestartRetryTran();
                }
                /// <summary>
                /// 锁事务超时
                /// </summary>
                public void TranLockOverTime()
                {
                        RpcTranLogic.TranLockOverTime();
                }
                /// <summary>
                /// 申请事务
                /// </summary>
                /// <param name="obj"></param>
                /// <param name="source"></param>
                public void ApplyTran(ApplyTran obj, MsgSource source)
                {
                        TransactionCollect.ApplyTransaction(source, obj);
                }
                /// <summary>
                /// 强制回滚事务
                /// </summary>
                /// <param name="obj"></param>
                public void ForceRollbackTran(ForceRollbackTran obj)
                {
                        TransactionCollect.ForceRollbackTran(obj.TranId);
                }
                /// <summary>
                /// 回滚事务
                /// </summary>
                /// <param name="obj"></param>
                public void RollbackTran(RollbackTran obj)
                {
                        TransactionCollect.RollbackTran(obj.TranId);
                }
                /// <summary>
                /// 设置事务的扩展参数
                /// </summary>
                /// <param name="obj"></param>
                public void SetTranExtend(SetTranExtend obj)
                {
                        TransactionCollect.SetTranExtend(obj.TranId, obj.Extend);
                }
                /// <summary>
                /// 提交事务
                /// </summary>
                /// <param name="obj"></param>
                public void SubmitTran(SubmitTran obj)
                {
                        TransactionCollect.SubmitTran(obj.TranId);
                }
                /// <summary>
                /// 获取事务状态
                /// </summary>
                /// <param name="obj"></param>
                /// <returns></returns>
                public RpcTranState GetTranState(GetTranState obj)
                {
                        return RpcTranLogic.GetTranState(obj.TranId);
                }
                /// <summary>
                /// 结束事务
                /// </summary>
                /// <param name="obj"></param>
                public void EndTran(EndTran obj)
                {
                        TransactionCollect.EndTran(obj.TranId);
                }
                /// <summary>
                /// 添加事务日志
                /// </summary>
                /// <param name="add"></param>
                /// <param name="source"></param>
                /// <returns></returns>
                public Guid AddTranLog(AddTranLog add, MsgSource source)
                {
                        return TransactionCollect.AddTranLog(add.TranId, add.TranLog, source);
                }
        }
}
