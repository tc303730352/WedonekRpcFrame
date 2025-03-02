using RpcCentral.Collect.Controller;

namespace RpcCentral.Collect
{
    public interface IRpcServerConfigCollect
    {
        RpcServerConfigController Get (long rpcMerId, long sysTypeId);
        void Refresh (long sysTypeId);
        void Refresh (long rpcMerId, long sysTypeId);
    }
}