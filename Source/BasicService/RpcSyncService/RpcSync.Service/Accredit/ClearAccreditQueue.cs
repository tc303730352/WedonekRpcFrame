using RpcSync.Service.Interface;
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
    internal class ClearAccreditQueue : IClearAccreditQueue
    {
        private readonly IDataQueueHelper<string> _ClearQueue = null;
        private readonly IAccreditKeyCollect _AccreditKey;
        private readonly AccreditType _AccreditType;

        public ClearAccreditQueue (ISysConfig config, IAccreditKeyCollect accreditKey)
        {
            this._AccreditKey = accreditKey;
            this._AccreditType = config.GetConfigVal<AccreditType>("sync:accredit:AccreditType", AccreditType.redis);
            this._ClearQueue = new RedisBatchDataQueue<string>("ClearAccredit", this._Clear, 100, 100, 20);
        }
        /// <summary>
        /// 清理授权
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fail"></param>
        private void _Clear (QueueData<string>[] data, Action<QueueData<string>, ErrorException> fail)
        {
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                IAccreditCollect service = scope.Resolve<IAccreditCollect>(this._AccreditType.ToString());
                data.ForEach(c =>
                {
                    if (service.Get(c.value, out IAccreditToken token) && token.Cancel())
                    {
                        _ = this._AccreditKey.TryRemove(token.CheckKey, token.AccreditId);
                    }
                });
            }
        }
        public void Add (string accreditId)
        {
            this._ClearQueue.AddQueue(accreditId);
        }
        public void Add (string[] accreditId)
        {
            this._ClearQueue.AddQueue(accreditId);
        }
    }
}
