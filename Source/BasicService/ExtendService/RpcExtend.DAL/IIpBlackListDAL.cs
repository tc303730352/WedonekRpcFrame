using RpcExtend.Model;

namespace RpcExtend.DAL
{
    public interface IIpBlackListDAL
    {
        IpBlack[] GetIpBlacks (long rpcMerId, string sysType, long beginVer, long endVer);
        long GetMaxVer (long rpcMerId, string sysType);
    }
}