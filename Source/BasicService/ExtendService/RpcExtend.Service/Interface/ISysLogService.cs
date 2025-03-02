using WeDonekRpc.ExtendModel.SysError;
using WeDonekRpc.Model;

namespace RpcExtend.Service.Interface
{
    public interface ISysLogService
    {
        void SaveSysLog (SysErrorLog[] logs, MsgSource source);
    }
}