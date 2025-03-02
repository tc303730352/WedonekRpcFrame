using RpcExtend.Collect;
using RpcExtend.Service.Interface;
using WeDonekRpc.Client.Attr;
using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Helper;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Service
{
    [ClassLifetimeAttr(ClassLifetimeType.SingleInstance)]
    internal class TraceService : ITraceService
    {
        private readonly ITraceCollect _Trace;
        private readonly ITraceLogCollect _TraceLog;

        public TraceService (ITraceCollect trace, ITraceLogCollect traceLog)
        {
            this._Trace = trace;
            this._TraceLog = traceLog;
        }

        public void Add (SysTraceLog[] logs, MsgSource source)
        {
            logs.ForEach(c =>
            {
                this._Trace.Add(c, source);
                this._TraceLog.Add(c, source);
            });
        }
    }
}
