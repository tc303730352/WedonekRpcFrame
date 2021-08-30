using System;

using RpcCacheClient.Helper;

using RpcSyncService.Collect;
using RpcSyncService.Model;

using RpcHelper;

namespace RpcSyncService.Tran
{
        /// <summary>
        /// 事务回滚
        /// </summary>
        internal class TranRollbackQueue
        {
                private static readonly IDataQueueHelper<Guid> _RollbackQueue = null;

                static TranRollbackQueue()
                {
                        string name = string.Concat("TranRollBack_", RpcClient.RpcClient.CurrentSource.RegionId);
                        _RollbackQueue = new RedisDataQueue<Guid>(name, _Rollback);
                }

                private static void _Rollback(Guid tranId)
                {
                        RegTranState tran = TransactionCollect.GetTranState(tranId);
                        if (tran.IsEnd)
                        {
                                return;
                        }
                        Guid[] ids = TransactionCollect.TranRollbackLock(tranId);
                        tran.Refresh();
                        if (ids.Length > 0)
                        {
                                SingleTranRollbackQueue.AddQueue(ids);
                        }
                }

                public static void AddQueue(RegTranState tran)
                {
                        _RollbackQueue.AddQueue(tran.MainTranId);
                }
                public static void AddQueue(Guid[] tranId)
                {
                        _RollbackQueue.AddQueue(tranId);
                }
        }
}
