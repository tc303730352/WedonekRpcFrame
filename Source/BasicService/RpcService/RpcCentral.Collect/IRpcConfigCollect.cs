using RpcCentral.Collect.Controller;

namespace RpcCentral.Collect
{
    public interface IRpcConfigCollect
    {
        RpcConfigController Get (long rpcMerId, long sysTypeId);
        void Refresh (long rpcMerId, long sysTypeId);
        void RefreshVerNum (long rpcMerId, long sysTypeId, int verNum);
    }
}