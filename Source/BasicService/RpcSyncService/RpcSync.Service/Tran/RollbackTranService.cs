using RpcSync.Collect;
using RpcSync.Model;
using RpcSync.Service.Interface;
using WeDonekRpc.CacheClient.Helper;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Model;

namespace RpcSync.Service.Tran
{
    internal class RollbackTranService : IRollbackTranService
    {
        private readonly IDataQueueHelper<long> _Queue = null;
        private readonly ISingleTranService _SingleTran;
        private readonly IIocService _Unity;
        public RollbackTranService (IIocService unity, ISingleTranService singleTran)
        {
            this._Unity = unity;
            this._SingleTran = singleTran;
            string name = string.Concat("TranRollBack_", RpcClient.CurrentSource.RegionId);
            this._Queue = new RedisBatchDataQueue<long>(name, this._Rollback, 20);
        }
        private void _Rollback (QueueData<long>[] tranId, Action<QueueData<long>, ErrorException> fail)
        {
            using (IocScope scope = this._Unity.CreateScore())
            {
                ITransactionCollect tranCollect = scope.Resolve<ITransactionCollect>();
                tranId.ForEach(c =>
                {
                    try
                    {
                        this._Rollback(c.value, tranCollect);
                    }
                    catch (Exception e)
                    {
                        fail(c, ErrorException.FormatError(e));
                    }
                });
            }
        }
        private void _Rollback (long tranId, ITransactionCollect tranCollect)
        {
            RegTranState tran = tranCollect.GetTranState(tranId);
            if (tran.TranStatus != TransactionStatus.待回滚)
            {
                return;
            }
            long[] ids = tranCollect.TranRollbackLock(tranId);
            if (!ids.IsNull())
            {
                this._SingleTran.AddQueue(ids);
            }
        }

        public void AddQueue (long tranId)
        {
            this._Queue.AddQueue(tranId);
        }
    }
}
