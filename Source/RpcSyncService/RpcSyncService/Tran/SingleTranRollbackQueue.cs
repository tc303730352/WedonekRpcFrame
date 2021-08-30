using System;

using RpcCacheClient.Helper;

using RpcModel;
using RpcModel.Tran.Model;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;
namespace RpcSyncService.Tran
{
        /// <summary>
        /// 处理单个事务的回滚
        /// </summary>
        internal class SingleTranRollbackQueue
        {
                private static readonly IDataQueueHelper<Guid> _RollbackQueue = null;
                static SingleTranRollbackQueue()
                {
                        string name = string.Concat("SingleTranRollBack_", RpcClient.RpcClient.CurrentSource.RegionId);
                        _RollbackQueue = new RedisDataQueue<Guid>(name, _Rollback);
                }
                private static void _Rollback(Guid tranId)
                {
                        TransactionDatum tran = TransactionCollect.GetTransaction(tranId);
                        if (tran == null)
                        {
                                throw new ErrorException("rpc.tran.get.error");
                        }
                        else if (!tran.IsRegTran)
                        {
                                tran.TranStatus = TransactionStatus.已回滚;
                        }
                        else if (tran.IsEnd || (tran.TranStatus != TransactionStatus.待回滚 && tran.TranStatus != TransactionStatus.回滚失败))
                        {
                                return;
                        }
                        else
                        {
                                IRemoteConfig config = new IRemoteConfig("Rpc_TranRollback", tran.SystemType, true, true)
                                {
                                        RpcMerId = tran.RpcMerId,
                                        RegionId = tran.RegionId
                                };
                                if (RpcClient.Collect.RemoteCollect.Send(config, new TranRollback
                                {
                                        TranId = tran.ParentId == Guid.Empty ? tran.Id : tran.ParentId,
                                        TranName = tran.TranName,
                                        SubmitJson = tran.SubmitJson,
                                        Extend = tran.Extend
                                }, out string error))
                                {
                                        tran.TranStatus = TransactionStatus.已回滚;
                                }
                                else
                                {
                                        tran.TranStatus = TransactionStatus.回滚失败;
                                        tran.RetryNum += 1;
                                        tran.ErrorCode = ErrorManage.GetErrorCode(error);
                                }
                        }
                        if (!TransactionCollect.RollbackResult(tran))
                        {
                                throw new ErrorException("rpc.tran.set.error");
                        }
                }

                public static void AddQueue(Guid[] tranId)
                {
                        _RollbackQueue.AddQueue(tranId);
                }
        }
}
