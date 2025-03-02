using WeDonekRpc.ExtendModel.Trace;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Interface
{
    public interface ITraceService
    {
        void Add (SysTraceLog[] logs, MsgSource source);
    }
}