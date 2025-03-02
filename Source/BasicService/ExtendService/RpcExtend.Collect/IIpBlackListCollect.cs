using RpcExtend.Model;

namespace RpcExtend.Collect
{
    public interface IIpBlackListCollect
    {
        IpBlack[] GetIpBlacks (long rpcMerId, string sysType, long beginVer, long endVer);
        long GetMaxVer (long rpcMerId, string sysType);
        void Refresh (long rpcMerId, string sysType);
    }
}