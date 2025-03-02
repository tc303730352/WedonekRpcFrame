using RpcCentral.Collect.Model;
using WeDonekRpc.Model;

namespace RpcCentral.Service.Interface
{
    public interface IRpcEvent
    {
        void Refresh(RpcTokenCache token, RefreshEventParam param);
    }
}