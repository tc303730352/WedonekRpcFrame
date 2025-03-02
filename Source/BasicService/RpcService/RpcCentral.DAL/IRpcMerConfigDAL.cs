using RpcCentral.Model;

namespace RpcCentral.DAL
{
    public interface IRpcMerConfigDAL
    {
        MerConfig GetConfig(long rpcMerId, long sysTypeId);
    }
}