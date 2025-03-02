using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Model;

namespace RpcExtend.Collect
{
    public interface ITraceCollect
    {
        void Add (SysTraceLog log, MsgSource source);
    }
}