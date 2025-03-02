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
using WeDonekRpc.Model.Tran.Model;

namespace RpcSync.Service.Tran
{
    internal class SingleTranService : ISingleTranService
    {
        private readonly IDataQueueHelper<long> _Queue = null;
        private readonly IIocService _Unity;
        private readonly ITranConfig _Config;
        private readonly IRemoteSendService _Send;
        public SingleTranService (IIocService unity,
            IRemoteSendService send,
            ITranConfig config)
        {
            this._Send = send;
            this._Unity = unity;
            this._Config = config;
            string name = string.Concat("SingleTran_", RpcClient.CurrentSource.RegionId);
            this._Queue = new RedisBatchDataQueue<long>(name, this._ExecTran, 50);
        }

        private void _ExecTran (QueueData<long>[] tranId, Action<QueueData<long>, ErrorException> fail)
        {
            using (IocScope scope = this._Unity.CreateScore())
            {
                ITransactionCollect tranCollect = scope.Resolve<ITransactionCollect>();
                tranId.ForEach(c =>
                {
                    try
                    {
                        this._ExecTran(c.value, tranCollect);
                    }
                    catch (Exception e)
                    {
                        fail(c, ErrorException.FormatError(e));
                    }
                });
            }
        }
        private void _ExecTran (long tranId, ITransactionCollect tranCollect)
        {
            TransactionDatum tran = tranCollect.GetTransaction(tranId);
            if (tran == null)
            {
                return;
            }
            if (tran.TranStatus == TransactionStatus.待回滚 || tran.TranStatus == TransactionStatus.回滚失败)
            {
                this._Rollback(tran, tranCollect);
            }
            else if (tran.TranStatus == TransactionStatus.已提交 &&
                ( tran.TranMode == RpcTranMode.Tcc || tran.TranMode == RpcTranMode.TwoPC ) &&
                ( tran.CommitStatus == TranCommitStatus.待提交 || tran.CommitStatus == TranCommitStatus.提交失败 ))
            {
                this._Commit(tran, tranCollect);
            }
        }
        private void _Commit (TransactionDatum tran, ITransactionCollect tranCollect)
        {
            if (this._SendDicate(tran, "Rpc_TranCommit", new TranCommit
            {
                TranId = tran.Id,
                TranName = tran.TranName,
                SubmitJson = tran.SubmitJson,
                Extend = tran.Extend
            }))
            {
                tran.CommitStatus = TranCommitStatus.已提交;
            }
            else if (tran.RetryNum >= this._Config.TranCommitRetryNum)
            {
                tran.CommitStatus = TranCommitStatus.提交错误;
            }
            else
            {
                tran.CommitStatus = TranCommitStatus.提交失败;
            }
            tranCollect.CommitResult(tran);
        }
        private bool _SendDicate<T> (TransactionDatum tran, string dicate, T data) where T : class
        {
            IRemoteConfig config = new IRemoteConfig(dicate, tran.SystemType, true, true)
            {
                RpcMerId = tran.RpcMerId,
                RegionId = tran.RegionId,
                IsProhibitTrace = true
            };
            if (this._Send.Send<T>(config, data, out string error))
            {
                return true;
            }
            else
            {
                tran.RetryNum += 1;
                tran.Error = error;
                return false;
            }
        }
        private void _Rollback (TransactionDatum tran, ITransactionCollect tranCollect)
        {
            if (tran.TranMode == RpcTranMode.NoReg)
            {
                tran.TranStatus = TransactionStatus.已回滚;
            }
            else if (this._SendDicate(tran, "Rpc_TranRollback", new TranRollback
            {
                TranId = tran.Id,
                TranName = tran.TranName,
                SubmitJson = tran.SubmitJson,
                Extend = tran.Extend
            }))
            {
                tran.TranStatus = TransactionStatus.已回滚;
            }
            else if (tran.RetryNum >= this._Config.TranRollbackRetryNum)
            {
                tran.TranStatus = TransactionStatus.回滚错误;
            }
            else
            {
                tran.TranStatus = TransactionStatus.回滚失败;
            }
            tranCollect.RollbackResult(tran);
        }

        public void AddQueue (long[] tranId)
        {
            this._Queue.AddQueue(tranId);
        }
    }
}
