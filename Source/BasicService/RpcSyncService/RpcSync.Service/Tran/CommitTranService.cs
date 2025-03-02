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
    internal class CommitTranService : ICommitTranService
    {
        private readonly IDataQueueHelper<long> _Queue = null;
        private readonly ISingleTranService _SingleTran;
        private readonly IIocService _Unity;
        public CommitTranService (IIocService unity, ISingleTranService singleTran)
        {
            this._Unity = unity;
            this._SingleTran = singleTran;
            string name = string.Concat("CommitTran_", RpcClient.CurrentSource.RegionId);
            this._Queue = new RedisBatchDataQueue<long>(name, this._Commit, 20);
        }
        private void _Commit (QueueData<long>[] tranId, Action<QueueData<long>, ErrorException> fail)
        {
            using (IocScope scope = this._Unity.CreateScore())
            {
                ITransactionCollect tranCollect = scope.Resolve<ITransactionCollect>();
                tranId.ForEach(c =>
                {
                    try
                    {
                        this._Commit(c.value, tranCollect);
                    }
                    catch (Exception e)
                    {
                        fail(c, ErrorException.FormatError(e));
                    }
                });
            }
        }
        private void _Commit (long tranId, ITransactionCollect tranCollect)
        {
            RegTranState tran = tranCollect.GetTranState(tranId);
            if (tran.TranStatus != TransactionStatus.已提交)
            {
                return;
            }
            long[] ids = tranCollect.LockStayCommitTran(tranId);
            if (!ids.IsNull())
            {
                this._SingleTran.AddQueue(ids);
            }
        }

        public void AddQueue (long tranId)
        {
            this._Queue.AddQueue(tranId);
        }

        public void Dispose ()
        {
            this._Queue.Dispose();
        }
    }
}
