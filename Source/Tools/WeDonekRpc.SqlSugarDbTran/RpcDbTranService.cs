using System.Collections.Concurrent;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;
using WeDonekRpc.SqlSugar;

namespace WeDonekRpc.SqlSugarDbTran
{
    [WeDonekRpc.Client.Attr.ClassLifetimeAttr(Client.Attr.ClassLifetimeType.SingleInstance)]
    internal class RpcDbTranService : IRpcTwoPcTran
    {
        private readonly ConcurrentDictionary<long, DbTran> _TranList = new ConcurrentDictionary<long, DbTran>();
        private readonly Timer _CheckTranTimeOut = null;

        private readonly IIocService _Ioc;
        private readonly IRpcTranService _TranService;
        public RpcDbTranService (IIocService ioc, IRpcTranService tranService)
        {
            this._Ioc = ioc;
            this._TranService = tranService;
            this._CheckTranTimeOut = new Timer(new TimerCallback(this._CheckTimeOut), null, 1000, 1000);
        }

        private void _CheckTimeOut (object state)
        {
            if (this._TranList.IsEmpty)
            {
                return;
            }
            int now = HeartbeatTimeHelper.HeartbeatTime;
            long[] keys = this._TranList.Keys.ToArray();
            keys.ForEach(a =>
            {
                if (this._TranList.TryGetValue(a, out DbTran tran) && tran.Lock(now))
                {
                    if (!this._TranService.GetTranState(tran.tanState, out TransactionStatus status, out string error))
                    {
                        if (!tran.CheckFail(error))
                        {
                            return;
                        }
                    }
                    else if (status == TransactionStatus.待提交 || status == TransactionStatus.已提交)
                    {
                        this._Commit(a);
                        return;
                    }
                    this._Rollback(a);
                }
            });
        }

        public IDisposable BeginTran (ICurTran cur)
        {
            if (this._TranList.ContainsKey(cur.TranId))
            {
                throw new ErrorException("rpc.tran.repeat");
            }
            ISqlClientFactory tran = this._Ioc.Resolve<ISqlClientFactory>();
            tran.BeginTran();
            if (!this._TranList.TryAdd(cur.TranId, new DbTran(tran.Current, cur)))
            {
                tran.RollbackTran(null);
                return null;
            }
            return tran;
        }
        private void _Commit (long id)
        {
            if (this._TranList.TryRemove(id, out DbTran tran))
            {
                tran.Commit();
            }
        }
        private void _Rollback (long id)
        {
            if (this._TranList.TryRemove(id, out DbTran tran))
            {
                tran.Callback();
            }
        }
        public void Commit (ICurTran cur)
        {
            this._Commit(cur.TranId);
        }

        public void Rollback (ICurTran cur)
        {
            this._Rollback(cur.TranId);
        }
    }
}
