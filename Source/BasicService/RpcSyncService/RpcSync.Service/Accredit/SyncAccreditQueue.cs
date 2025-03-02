using RpcSync.Service.Interface;
using RpcSync.Service.Model;
using WeDonekRpc.CacheClient.Helper;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;

namespace RpcSync.Service.Accredit
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class SyncAccreditQueue : ISyncAccreditQueue
    {
        private readonly IDataQueueHelper<SyncAccredit> _AccreditQueue = null;
        private readonly IAccreditKeyCollect _AccreditKey;
        private readonly AccreditType _AccreditType;
        public SyncAccreditQueue (ISysConfig config, IAccreditKeyCollect accreditKey)
        {
            this._AccreditKey = accreditKey;
            this._AccreditType = config.GetConfigVal<AccreditType>("sync:accredit:AccreditType", AccreditType.redis);
            this._AccreditQueue = new RedisBatchDataQueue<SyncAccredit>("SyncAccredit", this._SyncAccredit, 100, 100, 20);
        }
        /// <summary>
        /// 同步授权状态
        /// </summary>
        /// <param name="obj"></param>
        private void _SyncAccredit (QueueData<SyncAccredit>[] data, Action<QueueData<SyncAccredit>, ErrorException> fail)
        {
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                IAccreditCollect service = scope.Resolve<IAccreditCollect>(this._AccreditType.ToString());
                data.ForEach(c =>
                {
                    this._SyncAccredit(c.value, service);
                });
            }
        }
        private void _SyncAccredit (SyncAccredit obj, IAccreditCollect accredit)
        {
            TimeSpan? time = AccreditHelper.GetAccreditTime(obj.Expire);
            if (time.HasValue)
            {
                string old = this._AccreditKey.Set(ref obj, time.Value);
                if (old.IsNotNull() && old != obj.AccreditId)
                {
                    if (accredit.Get(old, out IAccreditToken token))
                    {
                        _ = token.Cancel();
                    }
                }
            }
        }

        public void Add (AccreditToken token)
        {
            SyncAccredit obj = new SyncAccredit
            {
                AccreditId = token.AccreditId,
                ApplyKey = token.CheckKey,
                Expire = token.Expire,
                StateVer = token.StateVer
            };
            this._AccreditQueue.AddQueue(obj);
        }
    }
}
