using RpcExtend.DAL;
using RpcExtend.Model.DB;
using WeDonekRpc.Client;
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
    internal class TraceLogCollect : ITraceLogCollect
    {
        private readonly IDelayDataSave<RpcTraceLog> _DelaySave;
        private readonly IIocService _Unity;
        public TraceLogCollect (IIocService unity)
        {
            this._Unity = unity;
            this._DelaySave = new DelayDataSave<RpcTraceLog>(this._Save, 2, 10);
        }
        private void _Save (ref RpcTraceLog[] datas)
        {
            using (IocScope score = this._Unity.CreateTempScore())
            {
                score.Resolve<IRpcTraceLogDAL>().Adds(datas);
            }
        }
        public void Add (SysTraceLog log, MsgSource source)
        {
            string result = log.Args.GetValueOrDefault("Result", string.Empty);
            string msgType = log.Args.GetValueOrDefault("MsgType", string.Empty);
            _ = log.Args.Remove("MsgType");
            _ = log.Args.Remove("Result");
            _ = log.Args.Remove("LocalId");
            _ = log.Args.Remove("SystemType");
            RpcTraceLog traceLog = log.ConvertMap<SysTraceLog, RpcTraceLog>();
            traceLog.BeginTime = log.Begin;
            traceLog.MsgType = msgType;
            traceLog.RegionId = source.RegionId;
            traceLog.ServerId = source.ServerId;
            traceLog.RemoteId = log.RemoteId.GetValueOrDefault();
            traceLog.SystemType = source.SystemType;
            traceLog.ReturnRes = result;
            traceLog.Id = IdentityHelper.CreateId();
            this._DelaySave.AddData(traceLog);
        }
    }
}
