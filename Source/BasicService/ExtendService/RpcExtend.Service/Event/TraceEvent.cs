using RpcExtend.Service.Interface;
using WeDonekRpc.Client.Interface;
using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Event
{
    /// <summary>
    ///链路日志
    /// </summary>
    internal class TraceEvent : IRpcApiService
    {
        private readonly ITraceService _Service;

        public TraceEvent (ITraceService service)
        {
            this._Service = service;
        }

        /// <summary>
        /// 节点链路
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="source"></param>
        public void SysTrace (SysTrace trace, MsgSource source)
        {
            this._Service.Add(trace.Logs, source);
        }
    }
}
