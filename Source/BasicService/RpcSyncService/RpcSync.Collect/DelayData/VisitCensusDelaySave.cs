using RpcSync.DAL;
using RpcSync.Model;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.Interface;

namespace RpcSync.Collect.DelayData
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class VisitCensusDelaySave : IVisitCensusDelaySave
    {
        private readonly IDelayDataSave<RpcVisitCensus> _SyncVisit;

        private readonly IIocService _Unity;
        public VisitCensusDelaySave (IIocService unity)
        {
            this._Unity = unity;
            this._SyncVisit = new DelayDataSave<RpcVisitCensus>(this._Save, this._Filter, 2, 10);
        }
        private void _Filter (ref RpcVisitCensus[] datas)
        {
            datas = datas.GroupBy(a => new
            {
                a.ServiceId,
                a.Dictate
            }).Select(a => new RpcVisitCensus
            {
                ServiceId = a.Key.ServiceId,
                Dictate = a.Key.Dictate,
                Success = a.Sum(c => c.Success),
                Fail = a.Sum(c => c.Fail),
                Concurrent = a.Sum(c => c.Concurrent)
            }).ToArray();
        }

        private void _Save (ref RpcVisitCensus[] datas)
        {
            using (IocScope score = this._Unity.CreateTempScore())
            {
                score.Resolve<IServerVisitCensusDAL>().Sync(datas);
            }
        }
        public void AddLog (RpcVisitCensus add)
        {
            this._SyncVisit.AddData(add);
        }
    }
}
