using RpcCentral.Collect.Controller;

namespace RpcCentral.Collect
{
    public interface IVerConfigCollect
    {
        RpcVerController GetVer (long rpcMerId, long sysTypeId, int verNum);
        void Refresh (long rpcMerId, long sysTypeId, int verNum);
    }
}