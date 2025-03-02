using RpcCentral.Model;

namespace RpcCentral.Collect
{
    public interface IRpcMerConfigCollect
    {
        MerConfig GetConfig(long rpcMerId, long sysTypeId);
    }
}