using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Model;

namespace RpcExtend.Collect
{
    public interface ITraceLogCollect
    {
        void Add (SysTraceLog log, MsgSource source);
    }
}