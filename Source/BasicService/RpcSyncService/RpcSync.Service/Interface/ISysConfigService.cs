using WeDonekRpc.Model;
using WeDonekRpc.Model.Config;

namespace RpcSync.Service.Interface
{
    public interface ISysConfigService
    {
        RemoteSysConfig GetSysConfig (MsgSource source);
    }
}