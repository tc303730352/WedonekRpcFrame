using RpcExtend.DAL;
using RpcExtend.Model.DB;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.Client.Ioc;
using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Helper;
using WeDonekRpc.Helper.IdGenerator;
using WeDonekRpc.Helper.Interface;
using WeDonekRpc.Model;

namespace RpcExtend.Collect.Collect
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class TraceCollect : ITraceCollect
    {
        private readonly IDelayDataSave<RpcTrace> _DelaySave;
        private readonly IIocService _Unity;
        public TraceCollect (IIocService unity)
        {
            this._Unity = unity;
            this._DelaySave = new DelayDataSave<RpcTrace>(this._Save, 2, 10);
        }
        private void _Save (ref RpcTrace[] datas)
        {
            using (IocScope score = this._Unity.CreateTempScore())
            {
                score.Resolve<IRpcTraceListDAL>().Adds(datas);
            }
        }

        public void Add (SysTraceLog log, MsgSource source)
        {
            if (!log.ParentId.HasValue)
            {
                this._DelaySave.AddData(new RpcTrace
                {
                    TraceId = log.TraceId,
                    BeginTime = log.Begin,
                    Duration = log.Duration,
                    RpcMerId = source.RpcMerId,
                    RegionId = source.RegionId,
                    ServerId = source.ServerId,
                    SystemType = source.SystemType,
                    Dictate = log.Dictate,
                    Show = log.Show,
                    Id = IdentityHelper.CreateId()
                });
            }
        }
    }
}
