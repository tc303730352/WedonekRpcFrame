using System;

using RpcModel;

using RpcHelper;
namespace RpcSyncService.Model
{
        public class RegTranState
        {
                /// <summary>
                /// 事务ID
                /// </summary>
                public Guid Id
                {
                        get;
                        set;
                }
                /// <summary>
                /// 主事务Id
                /// </summary>
                public Guid MainTranId
                {
                        get;
                        set;
                }

                /// <summary>
                /// 过期时间
                /// </summary>
                public DateTime OverTime
                {
                        get;
                        set;
                }
                /// <summary>
                /// 事务状态
                /// </summary>
                public TransactionStatus TranStatus
                {
                        get;
                        set;
                }
                /// <summary>
                /// 是否截止
                /// </summary>
                public bool IsEnd
                {
                        get;
                        set;
                }
                public TranLevel Level { get;  set; }
                public DateTime AddTime { get; set; }

                public void CheckIsEnd()
                {
                        if (this.IsEnd)
                        {
                                throw new ErrorException("rpc.tran.already.end");
                        }
                        else if (this.TranStatus != TransactionStatus.执行中)
                        {
                                throw new ErrorException("rpc.tran.state.change");
                        }
                        else if (this.OverTime <= DateTime.Now)
                        {
                                throw new ErrorException("rpc.tran.overtime");
                        }
                }
                public void CheckState()
                {
                        if (this.IsEnd)
                        {
                                throw new ErrorException("rpc.tran.already.end");
                        }
                        else if (this.TranStatus == TransactionStatus.待回滚)
                        {
                                throw new ErrorException("rpc.tran.state.change");
                        }
                }
                public void CheckIsAllowRollback()
                {
                        if (this.IsEnd)
                        {
                                throw new ErrorException("rpc.tran.already.end");
                        }
                        else if (this.TranStatus != TransactionStatus.已提交)
                        {
                                throw new ErrorException("rpc.tran.state.change");
                        }
                        else if (this.OverTime <= DateTime.Now)
                        {
                                throw new ErrorException("rpc.tran.overtime");
                        }
                }
                public void CheckIsExec()
                {
                        if (this.IsEnd)
                        {
                                throw new ErrorException("rpc.tran.already.end");
                        }
                        else if (this.TranStatus != TransactionStatus.已提交)
                        {
                                throw new ErrorException("rpc.tran.state.error",string.Concat("status=",(int)this.TranStatus));
                        }
                        else if (this.OverTime <= DateTime.Now)
                        {
                                throw new ErrorException("rpc.tran.overtime");
                        }
                }

                public void Refresh()
                {
                        RpcClient.RpcClient.Cache.Remove(string.Concat("TranState_", this.Id.ToString("N")));
                }
        }
}
