using RpcSync.Service.Interface;
using WeDonekRpc.Client;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;

namespace RpcSync.Service.Accredit
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class RefreshAccreditQueue : IRefreshAccreditQueue
    {
        private readonly DelayDataSave<string> _RefreshQueue = null;
        private readonly IAccreditKeyCollect _AccreditKey;
        private readonly AccreditType _AccreditType;

        public RefreshAccreditQueue (ISysConfig config, IAccreditKeyCollect accreditKey)
        {
            this._AccreditKey = accreditKey;
            this._AccreditType = config.GetConfigVal<AccreditType>("sync:accredit:AccreditType", AccreditType.redis);
            this._RefreshQueue = new DelayDataSave<string>(this._Refresh, _FilterData, 60, 100);
        }
        /// <summary>
        /// 去重
        /// </summary>
        /// <param name="datas"></param>
        private static void _FilterData (ref string[] datas)
        {
            datas = datas.Distinct().ToArray();
        }
        /// <summary>
        /// 刷新授权状态
        /// </summary>
        /// <param name="accredits"></param>
        private void _Refresh (ref string[] accredits)
        {
            using (IocScope scope = RpcClient.Ioc.CreateScore())
            {
                IAccreditCollect service = scope.Resolve<IAccreditCollect>(this._AccreditType.ToString());
                accredits.ForEach(a =>
                {
                    if (service.Get(a, out IAccreditToken token))
                    {
                        TimeSpan? time = token.Refresh();
                        if (time.HasValue)
                        {
                            this._AccreditKey.Renewal(token, time.Value);
                        }
                    }
                });
            }
        }
        public void Add (string accreditId)
        {
            this._RefreshQueue.AddData(accreditId);
        }
        public void Add (string[] accreditId)
        {
            this._RefreshQueue.AddData(accreditId);
        }
    }
}
