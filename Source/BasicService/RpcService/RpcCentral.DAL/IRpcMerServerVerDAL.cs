namespace RpcCentral.DAL
{
    public interface IRpcMerServerVerDAL
    {
        int? GetCurrentVer (long rpcMerId, long systemTypeId);
    }
}